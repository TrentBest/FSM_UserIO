# Development

## Build

~~~bash
dotnet restore TheSingularityWorkshop.FSM_UserIO.sln
dotnet build TheSingularityWorkshop.FSM_UserIO.sln --configuration Release --no-restore
~~~

## Test

~~~bash
dotnet test TheSingularityWorkshop.FSM_UserIO.sln --configuration Release --no-build
~~~

## Package

~~~bash
dotnet pack TheSingularityWorkshop.FSM_UserIO.csproj --configuration Release --no-build --output ./artifacts
~~~

## Design rule

Do not add a concrete device API merely because it is convenient.

Before adding a contract, answer:

1. What datum or meaning does it own?
2. Which boundary is responsible for that meaning?
3. Is the behavior shared by multiple domains?
4. Could a platform adapter implement it without leaking platform types upward?
5. Can the contract be tested without a physical device?

If those answers are unclear, document the boundary before expanding the API.
