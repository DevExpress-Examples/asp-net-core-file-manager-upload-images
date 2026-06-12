<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/230396054/26.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T848532)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# File Uploader for ASP.NET Core - How to upload images

This example shows how to use the [DevExtreme FileUploader](https://js.devexpress.com/Documentation/Guide/UI_Components/FileUploader/) to upload images and display links for these images in an ASP.NET Core MVC application with chunked upload support. 


## Files to look at:

* [Index.cshtml](./ASP.NET%20Core/Views/Home/Index.cshtml)
* [FileUploadController.cs](./ASP.NET%20Core/Controllers/FileUploadController.cs)
* [HomeController.cs](./ASP.NET%20Core/Controllers/HomeController.cs)
* [_Layout.cshtml](./ASP.NET%20Core/Views/Shared/_Layout.cshtml)

## Implementation Details

> **Note** The project targets .NET 8+ and uses modern ASP.NET Core MVC architecture with System.Text.Json for JSON serialization.

1) [Configure a Visual Studio Project](https://docs.devexpress.com/AspNetCore/401026/devextreme-based-controls/get-started/configure-a-visual-studio-project).

2) Add the [FileUploader](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder) component to your MVC View. Specify its [Name](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder.Name.overloads) and [UploadUrl](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder.UploadUrl.overloads) properties.

    ```cs
    @(Html.DevExtreme().FileUploader()
        .Name("uploadedFile")
        .ChunkSize(200000)
        .Multiple(true)
        .AllowedFileExtensions(new[] { ".jpg", ".jpeg", ".gif", ".png" })
        .MaxFileSize(4000000)
        .UploadUrl(Url.Action("Upload", "FileUpload"))
    ```

3) Create a dedicated controller for file upload operations and implement the [Chunk Upload](https://js.devexpress.com/jQuery/Documentation/Guide/UI_Components/FileUploader/Upload_Files/Server-Side_Implementation_in_ASP.NET/#Chunk_Upload) technique. The controller uses System.Text.Json for JSON deserialization.

4) Add the token value to the file uploader's requests because ASP.NET Core validates the [Antiforgery Token](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery) in POST requests. 

    ```js
    .UploadCustomData(new JS("{__RequestVerificationToken: document.getElementsByName('__RequestVerificationToken')[0].value}"))
    ```

5) The FileUploadController processes chunked uploads using System.Text.Json for metadata deserialization and handles file validation, temporary file management, and final file processing.

    ```cs
    [HttpPost]
    public IActionResult Upload(IFormFile uploadedFile, string chunkMetadata)
    {
        var tempPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        var resultImageUrl = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(chunkMetadata))
            {
                var metaDataObject = JsonSerializer.Deserialize<ChunkMetadata>(chunkMetadata);
                // Process chunk upload...
            }
        }
        catch
        {
            return BadRequest();
        }
        return new JsonResult(new { imgUrl = resultImageUrl });
    }
    ```

6) (*Optional*) Handle the [FileUploader.OnUploaded](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder.OnUploaded.overloads) event and get an image URL from the *args.request.response* property. 

    ```js
    function OnUploaded(args) {
            let imgUrl = JSON.parse(args.request.response).imgUrl;
            let imgName = args.file.name;
            let imagesContainer = document.getElementById("imagesContainer");
            imagesContainer.appendChild(createLink(imgName, imgUrl));
        }
    function createLink(imgName, url) {
            let link = document.createElement("A");
            link.innerText = imgName;
            link.href = url;
            return link;
        }
    ```

## Documentation

[Upload Files: Server-Side Implementation in ASP.NET](https://js.devexpress.com/Documentation/Guide/UI_Components/FileUploader/Upload_Files/Server-Side_Implementation_in_ASP.NET/)

## More Examples

* [Angular File Uploader - How to upload images using ASP.NET Core API Controller](https://github.com/DevExpress-Examples/Angular-File-Uploader-How-to-upload-images-using-ASP.NET-Core-API-Controller)

* [DataGrid - How to use FileUploader in an edit form](https://github.com/DevExpress-Examples/DataGrid---How-to-use-FileUploader-in-an-edit-form)
<!-- feedback -->
## Does This Example Address Your Development Requirements/Objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-core-file-manager-upload-images&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-core-file-manager-upload-images&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
