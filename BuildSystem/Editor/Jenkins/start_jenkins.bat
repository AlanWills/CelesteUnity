@echo off
:: 1. Modify to where you want your jobs and workspaces to be
set JENKINS_HOME=E:\Jenkins-Workspaces

:: 2. Modify to the install location of Jenkins and your desired port
java -jar "C:\Program Files\Jenkins\jenkins.war" --httpPort=1119