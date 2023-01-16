// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function SubmitWithSweetAlert(url, data, successText, postUrl)
{  
    $.ajax({
      type: "POST",
      url: url,
      dataType: 'json',   
      contentType: 'application/x-www-form-urlencoded; charset=UTF-8',   
      data: data,  
      beforeSend: function ()
      {
            
      },
      error: function (xhr, ajaxOptions, thrownError)
      {
        swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
      },
      success: function (data)
      {
        swal({ title: 'Complete', text: successText, icon: 'success' }); 
        setTimeout(function() 
        {
          window.location = postUrl;
        }, 2000);
      }
    });
}
 

function PromptWithSweetAlert(url, title, text, icon, type, successText, postUrl)
{
  swal({
    title: title,
    text: text,
    icon: icon,
    buttons: true,
    dangerMode: false
  }).then((answerYes) =>
  {
    if (answerYes)
    {
      $.ajax({
        type: type,
        url: url,
        beforeSend: function ()
        {

        },
        error: function (xhr, ajaxOptions, thrownError)
        {
          swal({ title: 'Failed', text: "Something went wrong, try again!", icon: 'error' });
        },
        success: function (data)
        {
          swal({ title: 'Complete', text: successText, icon: 'success' }); 
          setTimeout(function() 
          {
            window.location = postUrl;
          }, 2000);
        }
      });
    }
  });
}