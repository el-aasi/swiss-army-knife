# swiss-army-knife
![Banner](Images/Banner.png)

# Project Title

[![GitHub Actions Status](https://github.com/SCADAMINDS/swiss-army-knife/workflows/Build/badge.svg?branch=main)](https://github.com/SCADAMINDS/swiss-army-knife/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/SCADAMINDS/swiss-army-knife?branch=main&includeBuildsFromPullRequest=false)](https://github.com/SCADAMINDS/swiss-army-knife/actions)


Project Description

# csharplibrarytest

# TODO
- Build docs using doxygen or similar
- change key.snk ?
- Write introduction and running guide
- Setup CI & Github
- Publish to nuget on new releases
- Add [Pure] annotations where necessary
- Add null checks where necessary

## Tests missing
- Test HttpResponseMessageExtensions
- Test ObjectExtensions
- Test TaskExtensions


# CI
This project uses Github Actions to perform CI. It:
- Runs all tests and Roslyn Analyzers on the master branch, and on all PR's
- Runs code coverage via Codecov on all pull requests


# Contributing
You can pull or fork this project. It runs on dotnet core, so you should be able to run `dotnet test`
You cannot commit to `main` - but feel free to make a Pull Request.
Remember to add tests for your changes.

You build it via `dotnet tool restore`

dotnet cake --target=Build
dotnet cake --target=Test

see https://rehansaeed.com/the-fastest-nuget-package-ever-published-probably/

# Release Process


# Build documentation
Todo wait until DocFx supports .NET Core

For now, read inline documentation
