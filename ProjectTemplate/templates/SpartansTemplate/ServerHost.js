
var http = require('http');     // Alternate mechanisim: import * as http from 'http';
var url = require('url');       // import * as url from 'url';
var fs = require('fs');         // import * as fs from 'fs';
var path = require('path');     // import * as path from 'path';

var fileExtensions = {
     ".html":    "text/html",
     ".css":     "text/css",
     ".js":      "text/javascript",
     ".jpeg":    "image/jpeg",
     ".jpg":     "image/jpeg",
     ".png":     "image/png",
     ".json":    "application/json",  
 };

var mysql = require('mysql');

var con = mysql.createConnection({
  host: "sql.wpc-is.online",
  user: "sony",
  password: "sony3232",
  database: "db_sony"
});

var server = http.createServer(function(request, response) { 
    var pathname = url.parse(request.url).pathname;
    var filename;

    // console.log("\nRequest: " + request);
    // console.log("\nRequest.url: " + request.url.toString());
    // console.log("Pathname: " + pathname);

    if(pathname === "/") {
        filename = "WestValleySpartans.html"; 
    }
    else
        filename = path.join(process.cwd(), pathname);
        

if (pathname == "/import")
{

    searchData = request.url.split("=");
    value = searchData[1];
    //console.log(value);

    con.connect ( 

        function() {
     
        dynQuery = "SELECT `Cost` FROM Products WHERE `RecNumber` = '" + value + "'"; 
        con.query(dynQuery, function (err, result, fields) {
    //  console.log(result);
        response.end(JSON.stringify(result[0].Cost));

        }); // end con.query
        } // end function
    ); // end con.connect

} // end if
else

{   

 try {
        fs.accessSync(filename, fs.F_OK);
        var fileStream = fs.createReadStream(filename);
        var typeAttribute = fileExtensions[path.extname(filename)];

        // console.log("File extension: " + path.extname(filename));
        // console.log("Type Attribute: " + typeAttribute);

        response.writeHead(200, {'Content-Type': typeAttribute});
        fileStream.pipe(response);
    }
    catch(e) {
            console.log('File does not exist: ' + filename);
            response.writeHead(404, {'Content-Type': 'text/plain'});
            response.write('404 - File Not Found (' + filename + ')');
            response.end();
            return;
    }

}// end else introduced for sql data

    return;
    
}); // end var server

server.listen(5100);

console.log("Listening on port: 5100");