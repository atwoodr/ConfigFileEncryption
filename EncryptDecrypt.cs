using System;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace EncryptDecryptAppConfigFile
{
    public partial class EncryptDecrypt : Form
    {
        // Using constants here instead of config file settings so that we can deploy this tool as a single .exe file not dependant on .config files
        private static readonly string EncryptionProviderName = "PFEncryptionProvider";
        private static readonly string FileType = "*.config";

        private string FileName = string.Empty;
        private static bool isUsingXDTNamespace = false;

        private static List<string> specialXDTSections = new List<string>();

        public EncryptDecrypt()
        {
            specialXDTSections.Add("connectionStrings");
            specialXDTSections.Add("protectedAppSettings");
            specialXDTSections.Add("configProtectedData");

            InitializeComponent();

            // Set default radio button choice
            this.selectedRadioButton = radioButton_No;
        }

        public static void EncryptConfigSections(bool encrypt, List<string> configFileNames, bool saveChangesToConfigFile = true)
        {
            if (configFileNames != null)
            {
                foreach (string configFileName in configFileNames)
                {
                    EncryptConfigSections(encrypt, configFileName, saveChangesToConfigFile);
                }
            }
        }

        public static void EncryptConfigSections(bool encrypt, string configFileName, bool saveChangesToConfigFile = true)
        {
            try
            {
                // Prep the <configProtectedData> section for encryption/decryption with this tool
                RemoveConfigProtectedDataXDTTransformXMLAttribute(configFileName, encrypt);

                // Open the configuration file for use
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFileName };
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                // Initialize the list of config sections that will be encrypted/decrypted
                List<ConfigurationSection> configSections = InitializeConfigSections(configuration);

                foreach (ConfigurationSection configSection in configSections)
                {
                    if (!configSection.ElementInformation.IsLocked && !configSection.SectionInformation.IsLocked)
                    {
                        if (encrypt && !configSection.SectionInformation.IsProtected)
                        {
                            // Encrypt the config file section
                            configSection.SectionInformation.ProtectSection(EncryptionProviderName);
                        }

                        if (!encrypt && configSection.SectionInformation.IsProtected)
                        {
                            // Decrypt the config file section
                            configSection.SectionInformation.UnprotectSection();
                        }

                        configSection.SectionInformation.ForceSave = true;
                    }
                }

                if (saveChangesToConfigFile)
                {
                    // Save the current configuration
                    configuration.Save(ConfigurationSaveMode.Modified);
                }

                // Revert the initial changes to the <configProtectedData> section if required
                if (isUsingXDTNamespace)
                {
                    ReinstateConfigProtectedDataXDTTransformXMLAttribute(configFileName);
                }

                OpenConfigFileForViewing(configuration.FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("There was a problem using the tool. Are you sure you opened this .exe in Administrator mode?\n{0}", 
                    ex.ToString()));
            }
        }

        private static List<ConfigurationSection> InitializeConfigSections(Configuration configuration)
        {
            List<ConfigurationSection> configSections = new List<ConfigurationSection>();
            ConfigurationSection connectionStringsSection = configuration.GetSection("connectionStrings");
            ConfigurationSection protectedAppSettingsSection = configuration.GetSection("protectedAppSettings");

            if (connectionStringsSection != null)
                configSections.Add(connectionStringsSection);

            if(protectedAppSettingsSection != null)
                configSections.Add(protectedAppSettingsSection);

            return configSections;
        }
        
        /// <summary>
        /// This method prepares the config file for encryption or decryption by temporarily removing XML attributes that the 
        /// System.Configuration library cannot deal with while encrypting/decrypting.
        /// 
        /// This includes 
        /// 1) Removing any 'xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform"' attributes in the root node
        /// 2) Removing any 'xdt:Transform="Replace"' attributes from the <configProtectedData> XML section
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="encrypt"</param>
        private static void RemoveConfigProtectedDataXDTTransformXMLAttribute(string configFileName, bool encrypt)
        {
            XNamespace xdt = "http://schemas.microsoft.com/XML-Document-Transform";

            // Initialize the config file as an XML document
            XDocument xdoc = XDocument.Load(configFileName);
            
            // Check the root node for the XML namespace definition attribute
            XAttribute rootNodeXmlnsAttr = xdoc.Root.Attributes(XNamespace.Xmlns + "xdt")?.FirstOrDefault();

            // Check the <configProtectedData> XML section for the "xdt" XML attribute
            XAttribute configProtectedDataXDTAttr = xdoc.Root.Element("configProtectedData").Attributes(xdt + "Transform")?.FirstOrDefault();

            if (rootNodeXmlnsAttr != null && configProtectedDataXDTAttr != null)
            {
                // Recognize that this config file is using the XDT namespace
                isUsingXDTNamespace = true;

                // First, we need to loop through the sections of the config that we are not going to encrypt and temporarily 
                // add the xml namespace attribute for XDT so that each line in the config file doesn't have the definition auto-added as such
                // Note: This would not be necessary if we could suppress or override the XObjectEvent Changed event, but this appears to be impossible
                List<XElement> sectionsToAddXmlns = xdoc.Root.Elements().Where(section => !specialXDTSections.Contains(section.Name.LocalName)).ToList();

                // Iterate through each section and temporarily add the xml namespace attribute
                foreach (XElement section in sectionsToAddXmlns)
                {
                    if (section != null && section is XElement)
                    {
                        section.Add(new XAttribute(XNamespace.Xmlns + "xdt", xdt));
                    }
                }

                // Remove the 'xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform"' attribute from the configuration node
                rootNodeXmlnsAttr.Remove();

                // Remove the xdt:Transform attribute from the <configProtectedData> section of the config file
                configProtectedDataXDTAttr.Remove();
                
                // Now remove the <connectionStrings> and <protectedAppSettings> sections' attributes
                xdoc.Root.Element("connectionStrings")?.RemoveAttributes();
                xdoc.Root.Element("protectedAppSettings")?.RemoveAttributes();

                // Re-add the "configProtectionProvider" attribute to the <connectionStrings> and <protectedAppSettings> sections
                if (!encrypt)
                {
                    xdoc.Root.Element("connectionStrings")?.Add(new XAttribute("configProtectionProvider", EncryptionProviderName));
                    xdoc.Root.Element("protectedAppSettings")?.Add(new XAttribute("configProtectionProvider", EncryptionProviderName));
                }

                // Save the file before continuing
                xdoc.Save(configFileName);
            }
        }

        /// <summary>
        /// This method reinstates the XML attributes removed by the RemoveConfigProtectedDataXDTTransformXMLAttribute() method.
        /// (To be used once the encryption/decryption process has completed)
        /// </summary>
        /// <param name="configFileName"></param>
        private static void ReinstateConfigProtectedDataXDTTransformXMLAttribute(string configFileName)
        {
            XNamespace xdt = "http://schemas.microsoft.com/XML-Document-Transform";

            // Initialize the config file as an XML document
            XDocument xdoc = XDocument.Load(configFileName);
            
            // Reinstate the xmlns attribute on the configuration root node
            xdoc.Root.Add(new XAttribute(XNamespace.Xmlns + "xdt", xdt));

            // Find every section where we need to remove the temporarily added xml namespace attribute
            List<XElement> sectionsWithXmlns = xdoc.Root.Elements().Where(section => !specialXDTSections.Contains(section.Name.LocalName)).ToList();

            // Remove the temporarily added xml namespace attribute for each of these sections
            sectionsWithXmlns.Attributes().Where(a => a.Name.LocalName == "xdt")?.Remove();

            // Reinstate the xdt:Transform attribute to the sections of the config file in the "specialXDTSections" list
            XAttribute xdtTransformReplace = new XAttribute(xdt + "Transform", "Replace");

            foreach (string sectionName in specialXDTSections)
            {
                xdoc.Root.Element(sectionName)?.Add(xdtTransformReplace);
            }

            // Save the file
            xdoc.Save(configFileName);

            // Clear the flag
            isUsingXDTNamespace = false;
        }

        /// <summary>
        /// Attempts to open the file in notepad++, but opens in notepad of notepad++ isn't installed
        /// </summary>
        /// <param name="filePath"></param>
        private static void OpenConfigFileForViewing(string filePath)
        {
            try
            {
                Process.Start("notepad++.exe", filePath);
            }
            catch (Exception)
            {
                try
                {
                    Process.Start("notepad.exe", filePath);
                }
                catch { }
            }
        }

        private void ChangeFilesEncryption(bool encrypt, bool saveChangesToConfigFile = true)
        {
            if (File.Exists(FileName))
            {
                if (this.selectedRadioButton.Name.Contains("Yes"))
                {
                    List<string> configFilesInSelectedFolder = GetConfigFilenamesInSelectedFolder(FileName);
                    EncryptConfigSections(encrypt, configFilesInSelectedFolder, saveChangesToConfigFile);
                }
                else
                {
                    EncryptConfigSections(encrypt, FileName, saveChangesToConfigFile);
                }
            }
            else
            {
                MessageBox.Show("File doesn't exist");
            }
        }

        private List<string> GetConfigFilenamesInSelectedFolder(string fileName)
        {
            string directoryPath = Path.GetDirectoryName(fileName);
            List<string> configFilesInFolder = new List<string>(Directory.GetFiles(directoryPath, FileType));

            // Exclude packages.config from the list
            configFilesInFolder.RemoveAll(PackagesConfigPredicate);

            // Exclude *.Debug.config and *.Release.config from the list
            configFilesInFolder.RemoveAll(DebugConfigPredicate);
            configFilesInFolder.RemoveAll(ReleaseConfigPredicate);

            return configFilesInFolder;
        }

        private static bool PackagesConfigPredicate(String s)
        {
            return s.ToLower().EndsWith("packages.config");
        }

        private static bool DebugConfigPredicate(String s)
        {
            return s.ToLower().EndsWith(".debug.config");
        }

        private static bool ReleaseConfigPredicate(String s)
        {
            return s.ToLower().EndsWith(".release.config");
        }

        private void cmdOpen_Click_1(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            // This produces ".config files|*.config" if the config file setting = "*.config"
            openFileDialog1.Filter = FileType.Substring(1) + " files|" + FileType;

            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            FileName = openFileDialog1.FileName;
            txtEncryption.Text = FileName;
        }

        private void cmdEncrypt_Click_1(object sender, EventArgs e)
        {
            // Encrypt
            ChangeFilesEncryption(true);
        }

        private void cmdDecrypt_Click_1(object sender, EventArgs e)
        {
            // Decrypt
            ChangeFilesEncryption(false);
        }

        private void radioButtonNo_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(sender as RadioButton);
        }

        private void radioButtonYes_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(sender as RadioButton);
        }

        private void radioButtonChecked(RadioButton rbSender)
        {
            if (rbSender == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            if (rbSender.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference to it.
                selectedRadioButton = rbSender;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
