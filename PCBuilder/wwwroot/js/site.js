// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//let createSpeciticationButton = document.getElementById("create-specification");
//let specificationDiv = document.getElementById("specifications");

//createSpeciticationButton.onClick = function () {
//    specificationDiv.innerHTML +
//        '<label for="inputSpecifications">Name</label>' +
//        '<input type ="text" id="inputSpecifications" class="form-control">' +
//        '<label for="inputSpecifications">Description</label>' +
//        '<input type="text" id="inputSpecifications" class="form-control">'
//}

function add(tpe) {
    //Create an input type dynamically.   
    var element = document.createElement("label");
    var element2 = document.createElement("input");
    //Assign different attributes to the element. 

    element2.id = "inputSpecifications";
    element2.className = "form-control"; // Really? You want the default value to be the type string?

    var foo = document.getElementById("specifications");
    //Append the element in page (in span).  
    foo.appendChild(element);
    foo.appendChild(element2);
}
document.getElementById("create-specification").onclick = function () {
    add();
};

