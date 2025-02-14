pipeline {
    agent any

    environment {
        DEPLOY_PATH = "C:\\inetpub\\wwwroot\\restapi" // Windows path
    }
    
    stages {
        
        stage('Buildd') {
            steps {
                script {
                    echo "Building project in Release mode..."
                }
                bat 'dotnet build --configuration Release' // Build only
            }
        }
        
        stage('Stop IIS') {
            steps {
                script {
                    echo "Stopping IIS server..."
                }
                bat 'iisreset /stop' // Stop IIS
            }
        }
        
        stage('Publish') {
            steps {
                script {
                    echo "Publishing project to ${DEPLOY_PATH}..."
                }
                bat '''
                dotnet publish --configuration Release --framework net8.0 ^
                --output "%DEPLOY_PATH%" ^
                /p:PublishSingleFile=false /p:SelfContained=false
                '''
            }
        }
        
        stage('Start IIS') {
            steps {
                script {
                    echo "Starting IIS server..."
                }
                bat 'iisreset /start' // Start IIS
            }
        }
    }
}
