canon-file-organiser

## Helper program to organise files downloaded from a Canon camera
### How to use
 - Build, and run the `.dll` file with `dotnet canon-file-organiser.dll {source_directory} {target_directory}`
 - In root directory, run `dotnet run {source_directory} {target_directory}`

### Notes
 - Will organise photos into directory with a date pattern like `yyyy-MM` with a sub-dreictory containing all raw files
 - Should recursively work through directories
 - Supports JPG and CR2 file types
 - Command line argument can be entered at runtime