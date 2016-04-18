//Rails3 link_to submit form compatibility
function jquery_link_to(){
  $('a.link_to_compat').click(function(event){
    //Stop default behavior of link.click()
    event.preventDefault();
    var form = $( document.createElement('form') );
    form.attr("method","post").attr("action", $(this)[0].href)

    // get submission method (post|delete)
    // Add input field so Rails knows the request is DELETE
    method = $(this)[0].dataset.method;

    if (method != 'post') {
      var field = $(document.createElement('input'));
      field.attr('type', 'hidden').attr('name', '_method').val(method);
      form.append(field);
    }
    form.submit();
    return false;
  });
}

function mootools_link_to(){
  $$('a.link_to_compat').addEvent('click', function(event){
    event.stop();
    var form = new Element('form', {'action' : this.href, 'method' : 'post'});

    method= this.getProperty('data-method');

    if (method != 'post') {
      var field = new Element('input', {'type' : 'hidden', 'name' : '_method', 'value' : method});
      form.injectInside(field);
    }

    document.body.appendChild(form);
    form.submit();
    return false;
  });
}

function jQueryLoaded(){
  return (typeof(jQuery) != 'undefined');
}

function bind_link_to_for_framework(){
  if(jQueryLoaded()){
    $('document').ready(function(){
      jquery_link_to();
    });
  }
  else{
    window.addEvent('domready', function() {
      mootools_link_to();
    });
  }
}

//Prototype for submitting a div via link_to
// Note: Only works for AJAX async requests
// Example: link_to '/', :submit=>'id_of_div', :remote => true
function bind_link_to_submit_element_as_form(){
  document.observe("dom:loaded", function() {
    $$('a[submit]').each(function(link){
      Element.observe(link, 'click', function(event){
        // Cancel default behavior
        Event.stop(event);

        // Serialize div as form for params
        div_to_submit = $(this.readAttribute('submit'));
        params = Form.serialize(div_to_submit);

        // Make a new request to href with serialized div
        new Ajax.Request(this.readAttribute('href'), {
          method: this.readAttribute('data-method') || 'post',
          parameters: params,
          evalScripts: true,

          onComplete:    function(request) { this.fire("ajax:complete", request); },
          onSuccess:     function(request) { this.fire("ajax:success",  request); },
          onFailure:     function(request) { this.fire("ajax:failure",  request); }
       });
      });
    });
  });
}

