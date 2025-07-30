# ASP.NET Core DevExtreme File Upload Example

This example demonstrates how to implement file upload functionality using DevExtreme FileUploader in an ASP.NET Core MVC application with chunked upload support.

## Installation

Download the example and use Visual Studio 2022 (or later) or Visual Studio Code to open the project. This project targets .NET 8.0.

## Features

- **Chunked File Upload**: Large files are uploaded in 200KB chunks for better performance
- **Image Validation**: Only allows image files (.jpg, .jpeg, .gif, .png) up to 4MB
- **Anti-forgery Protection**: Includes CSRF token validation for security
- **Modern JSON Handling**: Uses System.Text.Json instead of Newtonsoft.Json
- **File Management**: Temporary file handling during upload process 

## Client-side resources and bundling

This project uses [NPM](https://www.npmjs.com/) and [Gulp.js](https://gulpjs.com/) to install client-side libraries. The project restores NPM packages before the first build. Then, Gulp bundles required scripts and CSS files into the resulting package during the first and every next build.

The project includes:
- DevExtreme 25.1.3
- DevExtreme.AspNet.Core 25.1.*
- jQuery 3.7.1

The resulted bundles will be located in the `wwwroot` folder:
* `css/vendor.css` - a file with all CSS styles.
* `css/icons` and `css/fonts` - folders that contain fonts and icons for DevExtreme themes.
* `js/vendor.js` - a file that contains all scripts.

The default bundle includes jQuery, Bootstrap, and DevExtreme.

### Add more 3rd-party libraries for additional features/components 

The main logic is located in the the `gulpfile.js` file at the root application level. The file contains two tasks:

* the `add-resources` task  

    * copies JavaScript files located in the `scripts` array and adds them to `vendor.js`. The script bundle is moved to `wwwroot\js`.
    * copies CSS styles located in the `styles` array and merges them into the `vendor.css` bundle. Then, this bundle is moved to `wwwroot\css`
    * copies DevExtreme `fonts` and `icons` folders from NPM  to `wwwroot\css`

*  the `clean` task removes all previously created files (`vendor.js` and `vendor.css`) and folders (`icons` and `fonts`)

If you need to include more features, you can uncomment one of the following sections:

* Gantt - scripts and styles for [dxGantt](https://js.devexpress.com/DevExtreme/Guide/UI_Components/Gantt/Getting_Started_with_Gantt/).
* Diagram - scripts and styles for [dxDiagram](https://js.devexpress.com/DevExtreme/Guide/UI_Components/Diagram/Getting_Started_with_Diagram/).
* Export  - scripts and styles for the exporting feature: [Export Data to Excel](https://js.devexpress.com/DevExtreme/Guide/UI_Components/DataGrid/Getting_Started_with_DataGrid/#Export_Data). 
* HtmlEditor - scripts and styles for [dxHtmlEditor](https://js.devexpress.com/DevExtreme/Guide/UI_Components/HtmlEditor/Overview/).
* Full Bundle - scripts and styles for all above mentioned features/components.  

## Code

Take a look at the following files of this example to see the required code:

**Controllers:**
- `Controllers/HomeController.cs` - Main controller with Index action
- `Controllers/FileUploadController.cs` - Handles file upload operations with chunked upload support

**Views:**
- `Views/Home/Index.cshtml` - Main page with DevExtreme FileUploader component
- `Views/Shared/_Layout.cshtml` - Layout with DevExtreme styles and scripts

**Models:**
- `Models/SampleOrder.cs` - Sample data model
- `Controllers/FileUploadController.cs` (ChunkMetadata class) - Metadata for chunked uploads

## Key Implementation Details

### FileUploader Configuration
```csharp
@(Html.DevExtreme().FileUploader()
    .ID("file-uploader")
    .Name("uploadedFile")
    .UploadCustomData(new JS("{__RequestVerificationToken: document.getElementsByName('__RequestVerificationToken')[0].value}"))
    .ChunkSize(200000)
    .Multiple(true)
    .AllowedFileExtensions(new[] { ".jpg", ".jpeg", ".gif", ".png" })
    .MaxFileSize(4000000)
    .OnUploaded("OnUploaded")
    .UploadMode(FileUploadMode.Instantly)
    .UploadUrl(Url.Action("Upload", "FileUpload"))
)
```

### Upload Controller
```csharp
[HttpPost]
public IActionResult Upload(IFormFile uploadedFile, string chunkMetadata)
{
    // Processes chunked uploads using System.Text.Json
    var metaDataObject = JsonSerializer.Deserialize<ChunkMetadata>(chunkMetadata);
    // File validation and processing...
    return new JsonResult(new { imgUrl = resultImageUrl });
}
```
- `Controllers/SampleDataController.cs` - API controller for sample data

**Models:**
- `Models/SampleData.cs` - Sample data model
- `Models/SampleOrder.cs` - Sample order model

**Views:**
- `Views/Home/Index.cshtml` - Main page with DevExtreme components
- `Views/Shared/_Layout.cshtml` - Layout template
- `Views/_ViewImports.cshtml` - Global imports
- `Views/_ViewStart.cshtml` - View start configuration

**Configuration:**
- `Program.cs` - Application entry point
- `Startup.cs` - Service configuration
- `gulpfile.js` - Build automation
- `package.json` - NPM dependencies
- `ASP.NET Core.csproj` - Project file

## Development server

Use the Visual Studio `Run (F5)` command or `dotnet run` command to run the project. The application will be available at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

## Further help

You can learn more about the ASP.NET Core components' syntax in our documentation: [Concepts](https://docs.devexpress.com/AspNetCore/400574/devextreme-based-controls/concepts/razor-syntax)
The client-side API is based on jQuery [jQuery documentation](https://api.jquery.com/) and described in the following topics: 
* [Get and Set Properties](https://js.devexpress.com/DevExtreme/Guide/jQuery_Components/Component_Configuration_Syntax/#Get_and_Set_Properties)
* [Call Methods](https://js.devexpress.com/DevExtreme/Guide/jQuery_Components/Component_Configuration_Syntax/#Call_Methods)
* [Get a UI Component Instance](https://js.devexpress.com/DevExtreme/Guide/jQuery_Components/Component_Configuration_Syntax/#Get_a_UI_Component_Instance)

To get more help on DevExtreme submit an issue in the [Support Center](https://supportcenter.devexpress.com/ticket/create)
