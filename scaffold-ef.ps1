$ErrorActionPreference = "Stop"

$name = "NodesDbContext"
$connectionString = "Server=(LocalDb)\TestDb;Integrated Security=True;Database=NodesDb"
$target = "Nodes.EfDbContext"


###
Write-Host "Setup..." -ForegroundColor Green
$tempProj = ".\.scaffolding\temp.csproj"
Remove-Item .\.scaffolding -Recurse -ErrorAction Ignore
New-Item .\.scaffolding -ItemType Directory -ErrorAction Ignore | Out-Null

@"
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>netcoreapp3.0</TargetFramework>
        <RootNamespace>$target.Entities</RootNamespace>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.0.1" />
        <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="3.0.1" />
    </ItemGroup>
</Project>
"@ | Out-File -FilePath $tempProj -Encoding utf8

dotnet tool install --global dotnet-ef --version 3.0.0
dotnet restore $tempProj


###
Write-Host "Scaffolding..." -ForegroundColor Green
dotnet ef dbcontext scaffold $connectionString `
    Microsoft.EntityFrameworkCore.SqlServer `
    -c $name `
    -o ..\$target\Entities `
    --force `
    --startup-project $tempProj `
    --project $tempProj


###
Write-Host "Cleaning..." -ForegroundColor Green

$allFiles = "$target\Entities\*.cs"
$contextFile = "$target\Entities\$name.cs"
$ctor = "public $name.*public $name.*?}"
$onConfiguring = " override void OnConfiguring\(DbContextOptionsBuilder optionsBuilder\)\s+{.+}\s+protected"
$partial = "(\n\s)+.*OnModelCreatingPartial.*"
$usings = @"
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
"@

Get-ChildItem -Path $allFiles | ForEach-Object {
    (Get-Content -path $_ -Raw) `
        -replace "public partial class", "public sealed class" `
        -replace " virtual ", " " `
    | Set-Content -Path $_ -NoNewline
}

(Get-Content -path $contextFile -Raw) `
    -replace $usings, "using Microsoft.EntityFrameworkCore;" `
    -replace "(?m)($partial)", "" `
    -replace "(?s)($ctor)", "public $name(DbContextOptions options) : base(options) { }" `
    -replace "(?s)($onConfiguring)", "" `
    | Set-Content -Path $contextFile -NoNewline

Remove-Item .\.scaffolding -Recurse -ErrorAction Ignore

Write-Host "Done." -ForegroundColor Green
