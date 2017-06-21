function postform(action, index) {
    var form = document.createElement("form");    
    form.method = "post";
    form.action = action;

    var input = document.createElement("input");
    input.name = "currentState";
    input.value = document.getElementById('serializedModel').value;
    input.type = "hidden";
    form.appendChild(input);

    input = document.createElement("input");
    input.name = "slotIndex";
    input.value = index;
    input.type = "hidden";
    form.appendChild(input);

    document.body.appendChild(form);
    form.submit();
    return form;
}