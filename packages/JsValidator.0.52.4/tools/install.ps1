param($installPath, $toolsPath, $package, $project)

$currentPostBuildCmds = $project.Properties.Item("PostBuildEvent").Value

$buildCmd = '"$(SolutionDir)packages\JsValidator.{0}\tools\jsvalidator.exe" "$(ProjectDir)Scripts\jsvalidator\config.js"' -f $package.Version

#cleanup old stuff
$newCmd = [regex]::Replace($currentPostBuildCmds, "(?m)^.*JsValidator\..*$", "").Trim("`r", "`n")

#install new stuff
$newCmd = $currentPostBuildCmds.Trim("`r", "`n") + "`r`n" + $buildCmd + "`r`n";
$project.Properties.Item("PostBuildEvent").Value = $newCmd