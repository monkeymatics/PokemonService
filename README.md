# PokemonService

Deployment instructions

Pre-requisites for deployment:

AWS CLI
Python

Installation instructions for the above can be found here - https://docs.aws.amazon.com/cli/latest/userguide/install-cliv2-windows.html

Once installed, you will need to configure the CLI, instructions for this can be found here - https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html
Ensure you enter eu-west-1 as your region

Yarn in also required and can be found here along with installation instructions - https://classic.yarnpkg.com/en/docs/install/#windows-stable

On a first-time deployment, the API must be deployed before the app gateway.

To deploy the API, open a powershell window in administrator mode and navigate to the root folder of the PokemonService repository, once here, run the following command:

. "deploy/Deployment-Funcs.ps1"; Deploy dev -Rebuild -BuildVersion 1

Be sure to increment the BuildVersion parameter on each deployment. When using CI/CD this can be automated

Once the API has deployed, the next step will be to deploy the AppGateway, navigate in powershell to the root of the PokemonAppGateway repository, once here, run the following command:

. "./Deployment-Funcs.ps1"; Deploy dev -Rebuild

This will deploy the APIGateway to AWS and publish the endpoints to make them visible.

Now you can use the service using a browser or software such as Postman, the URL formats are as follows:

Standard details:

{BaseAPIGatewayURL}/pokemon/{PokemonName}

Translated details:

{BaseAPIGatewayURL}/pokemon/translated/{PokemonName}
