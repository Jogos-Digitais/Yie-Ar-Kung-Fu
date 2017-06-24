function ajax(url, vars, callbackFunction) {

    if (window.XMLHttpRequest) {
        request = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        request = new ActiveXObject("Microsoft.XMLHTTP");
    }

  request.open("POST", url, true);
  request.setRequestHeader("Content-Type",
                           "application/x-www-form-urlencoded");

  request.onreadystatechange = function() {
    var done = 4, ok = 200;
    if (request.readyState == done && request.status == ok) {
      if (request.responseText) {
        callbackFunction(request.responseText);
      }
    }
  };
  request.send(vars);
}

function ajax_plus(url, vars, callbackFunction, addParam) {

    if (window.XMLHttpRequest) {
        request = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        request = new ActiveXObject("Microsoft.XMLHTTP");
    }

  request.open("POST", url, true);
  request.setRequestHeader("Content-Type",
                           "application/x-www-form-urlencoded");

  request.onreadystatechange = function() {
    var done = 4, ok = 200;
    if (request.readyState == done && request.status == ok) {
      if (request.responseText) {
        callbackFunction(request.responseText+"&lt;quote&gt;"+addParam+"&lt;/quote&gt;");
      }
    }
  };
  request.send(vars);
}

function echo_response(text)
{
	alert(text);
}

function no_response()
{
}

function dw_track(page_type, ontology, urs, useract, pid)
{
	var data = '';
	if(pid)
		data = data + '&pid=' + pid + '&prodtypid=8';
	if(urs)
		data = data + '&ursuid=' + urs + '&ursappid=46';
	var ts = new Date().getTime();
	var img = document.createElement('img');
	img.src = 'https://dw.cbsi.com/clear/c.gif?sid=19&edid=107&ptid=' + page_type + '&onid=' + ontology + '&useract=' + useract + '&ts=' + ts + data;
	img.width = 1;
	img.height = 1;
	document.getElementById('footer').appendChild(img);
}