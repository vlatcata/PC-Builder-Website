

function add(tpe) {
    var specificationlabel = document.createElement("label");
    var specificationinput = document.createElement("input");

    specificationlabel.id = "inputSpecifications";
    specificationlabel.textContent = "Name"

    specificationinput.className = "form-control";
    specificationinput.type = "text";
    specificationinput.placeholder = "Specification..";
    /*specificationinput.setAttribute("asp-for", "@Model.Specifications[i].Title");*/

    var descriptionlabel = document.createElement("label");
    var descriptioninput = document.createElement("input");

    descriptionlabel.id = "inputSpecifications";
    descriptionlabel.textContent = "Description";

    descriptioninput.className = "form-control";
    descriptioninput.type = "text";
    descriptioninput.placeholder = "Description..";
    /*descriptioninput.setAttribute("asp-for", "@Model.Specifications[i].Description");*/

    var foo = document.getElementById("specifications");
    foo.appendChild(specificationlabel);
    foo.appendChild(specificationinput);

    foo.appendChild(descriptionlabel);
    foo.appendChild(descriptioninput);
}
document.getElementById("create-specification").onclick = function () {
    add();
};
