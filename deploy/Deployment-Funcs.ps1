# Call from repository root e.g:
#   C:\Git\Merchant
# or
#   C:\Git\Transaction
#
# Example usage:
# . "deploy/Deployment-Funcs.ps1"; Build
# . "deploy/Deployment-Funcs.ps1"; Deploy dev
# Or
# . "deploy/Deployment-Funcs.ps1"; Deploy dev -Rebuild

function Build() {

    $json = loadJsonFile "deploy/projects.json"
    $projects = $json['projects']

    Push-Location "src"
    foreach ($proj in $projects) {
        Push-Location $proj
        
        dotnet restore # This seems to be required otherwise get 'dotnet-lambda execeutable not found' error
		Write-Host "Packaging lambda"
        dotnet lambda package `
            --configuration Release `
            --output-package "../../deploy/lambda/$proj.zip"
		
		Write-Host "Packaged lambda"
		
        if (!($? -eq "False")) {
            throw "Build failed for $proj"
        }

        Pop-Location
    }
    Pop-Location
}

function Deploy([string]$Environment, [switch]$Rebuild, [string]$BuildVersion) {
    $vars = loadVarsForEnvironment $Environment

    if ([string]::IsNullOrEmpty($BuildVersion)) {
        $BuildVersion = Get-Date -UFormat "%y%j%H%M%S"
    }

    if ($Rebuild) {
        Build
    }

    $profileName = $vars.ProfileName
    if (!$profileName) {
        $profileName = 'default'
    }

    aws s3 cp "deploy/lambda" "s3://$($vars['ArtifactsS3Bucket'])/$($vars['ArtifactsS3Prefix'])/$BuildVersion" `
        --recursive `
        --exclude "*" `
        --include "*.zip" `
        --profile $profileName
    
    aws s3 cp "deploy/templates" "s3://$($vars['ArtifactsS3Bucket'])/$($vars['ArtifactsS3Prefix'])/$BuildVersion" `
        --recursive `
        --exclude "*" `
        --include "*.yaml" `
        --profile $profileName

    $stackName = $vars.StackNamePrefix + $vars.StackName
	Write-Host (Get-Location)
    # Form the deployment command including each parameter in the var/EnvironmentName.json file
    # no-fail-on-empty-changeset because there may just be code updates, in which case the previous upload to S3 will be sufficient
    $deployCmd = "aws cloudformation deploy --profile $profileName --stack-name $stackName --capabilities CAPABILITY_IAM --no-fail-on-empty-changeset --template-file deploy/templates/pokemon-service.yaml --parameter-overrides"
    foreach ($key in $vars.Keys) {
        $deployCmd = $deployCmd + " $($key)=$($vars[$key])"
    }
	
    # This is used to force update of the lambda code
    $deployCmd = $deployCmd + " BuildVersion=$BuildVersion"

    # Print the command out for logging purposes and run the command
    $deployCmd
    Invoke-Expression $deployCmd # Powershell inception :B
}

function loadVarsForEnvironment([string]$Environment) {
    $varFile = "deploy/vars/$Environment.json"
    loadJsonFile $varFile
}

# Load a jsonfile as a Hashtable
function loadJsonFile([string] $filePath) {
    $obj = Get-Content -Raw -Path $filePath | ConvertFrom-Json
    objectToHashtable $obj
}

# Convert a CustomPsObject to a Hashtable
function objectToHashtable($Object) {
    $table = @{ }
    $Object.psobject.properties | ForEach-Object { $table[$_.Name] = $_.Value }

    $table
}