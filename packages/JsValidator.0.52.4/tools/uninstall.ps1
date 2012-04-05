param($installPath, $toolsPath, $package, $project)

$currentPostBuildCmds = $project.Properties.Item("PostBuildEvent").Value

$buildCmd = '"$(SolutionDir)packages\JsValidator.{0}\tools\jsvalidator.exe" "$(ProjectDir)Scripts\jsvalidator\config.js"' -f $package.Version

$newCmd = [regex]::Replace($currentPostBuildCmds, "(?m)^.*JsValidator\..*$", "").Trim("`r", "`n")
$project.Properties.Item("PostBuildEvent").Value = $newCmd