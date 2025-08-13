# GitHub Actions: From Zero to Hero Workshop

In this workshop we're going to cover GitHub Actions. For a tangentially related workshop of using GitHub Actions to deploy to Azure using Bicep, check out this repo: https://github.com/scottsauber/workshop-dotnet-azure-github-bicep

## Pre-requisites:

- Basic Git Knowledge
- GitHub Account
- Recommended: an editor with a GitHub Actions extension installed for intellisense (ie [VS Code](https://code.visualstudio.com/) with the [GitHub Actions extension](https://marketplace.visualstudio.com/items?itemName=GitHub.vscode-github-actions))
- Optional: .NET 9 installed locally to run the app locally

## Module 1 - Slides: CI/CD Pipelines

No hands-on work, just going over slides

## Module 2 - Slides: GitHub Actions

No hands-on work, just going over slides

## Module 3 - Hands On: Creating a PR Verify Workflow

1. Fork this repository if you haven't already.
1. Create a new branch
1. Create a `.github` folder in the root
1. Under the `.github` folder, create a `workflows` folder
1. Under the `workflows` folder, create a `pr-verify.yml` file
1. Paste the following into that file

   ```yml
   name: PR Verify

   on:
     pull_request:
       branches: ["main"]
     workflow_dispatch:

   jobs:
     build:
       name: PR Verify
       runs-on: ubuntu-22.04

       steps:
         - uses: actions/checkout@v4

         - name: Set up .NET Core
           uses: actions/setup-dotnet@v4
           with:
             dotnet-version: 9.0.x

         - name: Build with dotnet
           run: dotnet build --configuration Release

         - name: Test
           run: dotnet test --configuration Release --no-build
   ```

1. Commit, Push, and Open a PR
1. Do you think the GitHub Action will trigger when it's not committed on the `main` branch?
1. Scroll down to the bottom of the PR and notice you have a new status check called "PR Verify" and click on it
1. Watch the build succeed
1. Make a change that makes the code not compile. ie open the src/WorkshopDemo/Program.cs file and add a letter to the first line
1. Commit and push and watch it fail
1. Undo that change and let's make a test fail.
1. Open /src/WorkshopDemo.Core.Tests/Common/FileServiceTests.cs and change the assertion on line 19 to something like `result.ShouldBe("Hello world");`
1. Commit and push and watch it fail
1. Undo that change and commit and push
1. Do not merge it in yet

## Module 4 - Slides: Optimal GitHub settings

No hands-on work, just going over slides

## Module 5 - Hands On: Merge PR and Required Rule set

1. Merge the PR from Module 3
1. Setup a required rule set
1. Ruleset name: Main
1. Add Target: Include Default Branch
1. Check Restrict Deletions
1. Check Require Pull Request Before Merging
   1. Allow only Squash Merges (🌶️)
1. Check Require Status Checks to Merge
   1. Click Require Branches to be up to date
   1. Click Add Checks
   1. Search for PR Verify and check it
1. Check Block Force Pushes
1. Save Changes

## Module 6 - Hands On: Create a CI Workflow

1. Based on the PR Verify workflow, create one for the CI workflow (running after merge) called `ci.yml`. Make sure that `dotnet build` and `dotnet test` run as part of that.
   1. Hint: you want the `push` trigger not `pull_request`
1. Pssstttt... do we need to do that if we require branches to be up to date? Let's talk about it

## Module 7 - Slides: Reusable Workflows

No hands-on work, just going over slides

## Module 8 - Hands On: Reusable Workflows

1. Let's create a reusable workflow for PR Verify and CI to reduce duplication
1. Note: I don't normally do this for PR + CI, but will do it for deploys (see [here](https://github.com/scottsauber/workshop-dotnet-azure-github-bicep?tab=readme-ov-file#module-6---github-actions-hands-on) for more details).
1. Create a file called `reusable-build.yml`
1. Add the following to it:

   ```yml
   name: Reusable - Build

   on:
     workflow_call:
       inputs:
         dotnet_version:
           required: true
           type: string

   jobs:
     build:
       name: Reusable - Build
       runs-on: ubuntu-22.04

       steps:
         - uses: actions/checkout@v4

         - name: Set up .NET Core
           uses: actions/setup-dotnet@v4
           with:
             dotnet-version: ${{ inputs.dotnet_version }}

         - name: Build with dotnet
           run: dotnet build --configuration Release

         - name: Test
           run: dotnet test --configuration Release --no-build
   ```

1. In your pr-verify.yml, change the YAML to this:

   ```yml
   name: PR Verify

   on:
     pull_request:
       branches: ["main"]
     workflow_dispatch:

   jobs:
     build:
       name: PR Verify
       uses: ./.github/workflows/reusable-build.yml
       with:
         dotnet_version: 9.0.x
   ```

1. Make a similar change for `ci.yml`
1. Congratulations - now we have shared code between CI and PR
1. But what if we wanted this in a reusable repo...? 🤔

## Module 9 - Slides: Reusable Workflows in another repository

No hands-on work, just going over slides

## Module 10 - Hands On: Reusable Workflows in another repository

1. Make a new repository
1. Create a `.github/workflows/reusable-build.yml` file and copy it over
1. Change your `pr-verify` and `ci.yml` to point at the new repository instead of your local one

## Module 11 - Slides: Secrets

No hands-on work, just going over slides

## Module 12 - Hands On: Secrets

1. Create a `secrets-test.yml` file with the following YAML

   ```yml
   name: Echo
   on:
     pull_request:
       branches: ["main"]

   jobs:
     echo:
       name: Echo
       runs-on: ubuntu-latest
       steps:
         - name: Hello world
           run: echo ${{ secrets.SOME_SECRET }}
   ```

1. Go to the repository's Settings tab
1. Click on Secrets and Variables => Secrets
1. Click "New Repository Secret"
1. Give the name of SOME_SECRET and then a random value
1. Commit and push the workflow above and watch how it masks the value
