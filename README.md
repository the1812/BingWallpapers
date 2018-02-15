# BingWallpapers
A tool for downloading wallpapers from Bing homepage.

Resolution: `1920x1080`

## Settings
This app will show settings page at first run. Settings will be saved as `settings.json` in working folder.
> You can reset settings by deleting `settings.json`.
### Download Folder (`DownloadPath` in `settings.json`)
Configure where the wallpapers be saved.

**Default Value**: (Empty String)
### Filename Format (`FileNameFormat` in `settings.json`)
Configure the filename of each wallpaper. (No need to add `.jpg` at end)

**Default Value**: `${Year}-${Month}-${Day}-${Locale}`
#### Foramt Variables:
- `${Year}`: Year of wallpaper.
- `${Month}`: Month of wallpaper.
- `${Day}`: Day of wallpaper.
- `${Locale}`: Locale of wallpaper.
## Work silently
Start with the command line parameter `--silent`.

## Supported Language
- English (US) **(Default)**
- 简体中文

## Supported Download Sources
See [locales.json](https://github.com/the1812/BingWallpapers/blob/master/BingWallpapers/locales.json).

## Remarks
- This app couldn't have more than one instance. Trying to start a second instance will switch to the running instance and exit.
