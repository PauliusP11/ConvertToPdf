# PdfConverter

## Instrucions:
* Add Xceed licence key in launchSettings.json file.
* Run the app
* Post empty request to API

Docx sample is already hardcoded. You can post request with empty body:
https://localhost:20491/ConvertDocxToPdf

or add your test docx document to request body:
{"DocxData" : "abc..."}


### Additional comment:
When request is received document is converted twice for testing purposes. (To show it does not work parallel).

You can comment 47-49 lines in PdfConvertController.cs to convert it only once - and everything works fine.