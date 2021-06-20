# BugTracker

A web application for tracking project bugs and requested feature additions.

### Table of Contents

- [Setup](#setup)
- [Usage](#usage)

![Bug Tracker Dashboard](Bug_Tracker/Data/attachments/dashboard.png)

## Setup

1. Run the following commands in `Windows PowerShell` to get a copy of this project.

```shell
Invoke-WebRequest -Uri https://github.com/Chows23/BugTracker/archive/refs/heads/main.zip -OutFile BugTracker.zip;
Expand-Archive BugTracker.zip -DestinationPath '.\Bug Tracker\';
rm BugTracker.zip;
```

2. In Visual Studio, when the [wait cursor](https://en.wikipedia.org/wiki/Windows_wait_cursor) disappears, open the
NuGet Package Manager Console with `Tools -> NuGet Package Manager -> Package Manager Console`

There should be a message at the top of the console pane:

> Some NuGet packages are missing from this solution. Click to restore from your online package sources.

3. Click the `Restore` button on this message to install the missing packages
4. In the console, run the command `Update-Database`
5. Clean the solution with `Build -> Clean Solution`
6. Rebuild the solution with `Build -> Rebuild Solution`
7. Press `ctrl + F5` to run the project, it should open in your default browser

## Usage

There are several user accounts and projects with tasks included in the seed data.

| Username  | Role      |
| --------- | --------- |
| admin     | admin   |
| admin2    | admin   |
| manager   | manager   |
| manager2  | manager   |
| katherine | developer |
| elizabeth | developer |
| chows     | developer |
| anton     | developer |
| submitter | submitter   |
| submitter2| submitter   |

`P3nguin!` is the password for all user accounts.
