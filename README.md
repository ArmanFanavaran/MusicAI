# MusicAI
we must add iis_users to all folders security section listed in config bellow. so that the .net code can access the third party app to run it and the app can access its licence file as well

  "AnthemConfig": {
    "anthemScorePath": "C:\\Program Files\\AnthemScore\\AnthemScore.exe",
    "outPutDir": "C:\\Users\\Administrator\\Desktop\\AIOutput\\",
    "outPutFormat": ".musicxml",
    "inPutDir": "C:\\Users\\Administrator\\Desktop\\AIInput\\",
    "inPutFormat": ".wav",
    "appDataLocal": "C:\\Users\\Administrator\\AppData\\Local",
    "MaxCuncurrentRequestPerCore": 50,
    "MaxCoreParallelism": 2
  }

some times the license must be placed on path :   C:\Windows\System32\config\systemprofile\AppData\Local\AnthemScore instead of 
C:\\Users\\Administrator\\AppData\\Local. so we face with not found lincense file we can move license file to new location. 
we also can look for a file named : license_activation_code to find location of the license files

the build code of anthem score is in this link : https://www.lunaverus.com/download. we must install it in path : C:\\Program Files\\AnthemScore so the configs in .net app can find it and run it
