pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = '/tmp/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 'true'
        DEPLOY_PATH = "C:\\inetpub\\wwwroot\\restapi" // Windows path
    }
        
        stage('Restore') {
            steps {
                script {
                    echo "Restoring dependencies..."
                }
                bat 'dotnet restore' // Restore dependencies
            }
        }
        
        stage('Build') {
            steps {
                script {
                    echo "Building project in Release mode..."
                }
                bat 'dotnet build --configuration Release' // Build only
            }
        }
        
        stage('Publish') {
            steps {
                script {
                    echo "Publishing project to ${DEPLOY_PATH}..."
                }
                bat """
                dotnet publish --configuration Release \
                --framework net8.0 \
                --output "${DEPLOY_PATH}" \
                /p:PublishSingleFile=false \
                /p:SelfContained=false
                """
            }
        }
        stages {
        stage('Stop IIS') {
            steps {
                script {
                    echo "Stopping IIS server..."
                    bat 'iisreset /stop' // Stop IIS
                }
            }
        }
        
        stage('Start IIS') {
            steps {
                script {
                    echo "Starting IIS server..."
                    bat 'iisreset /start' // Start IIS
                }
            }
        }

        stage('Print Publish Location') {
            steps {
                script {
                    echo "Checking published files at ${DEPLOY_PATH}..."
                }
                bat "dir ${DEPLOY_PATH}" // Verify output
            }
        }
    }

    post {
        always {
            script {
                echo "Cleaning up workspace..."
            }
            cleanWs() // Cleanup
        }
    }
}
