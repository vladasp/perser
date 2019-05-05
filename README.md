# perser 
Windows service for monitoring and processing logs files

# Installation
1. Build the app
2. Copy folder contents to the permanent place
3. Run cmd
4. Run command to go to the permanent place
5. Run command to start the app with install parameter: start Parser.exe --install
6. The Parcer appear in the Windows services list 

# Denstallation
1. Run cmd
2. Run command to go to the permanent place
3. Run command to start the app with unistall parameter: start Parser.exe --unistall
4. The Parcer disappear in the Windows services list

# App configurations
  *int* **LogUpdateInterval** - parsing interval in minutes (default value 1);    
  *string* **InputDataFolder** - folder with files to process (default value "C:\\Parser\\InputFiles");    
  *string* **OutputDataFolder** - folder with parsing results file (default value "C:\\Parser\\Result");    
  *bool* **ShouldSaveProceedFiles** - sets whether to save processed files to "proceed" folder (default value true);
