$(document).ready(function() {
    $('#searchinput').keyup(function() {
        delay(function(){
          // the simplest query is _search?q=s
          //http://localhost:9200/sql_court_log/_search?q=s

          
          //add this to your yml to allow cross origin requests
         // http.cors.allow-origin: "/.*/"
         // http.cors.enabled: true
         var searchterm = $("#searchinput").val();
         var url = "http://localhost:9200/sql_court_log/_search?q=" + searchterm + "&size=20";
          $.ajax({
            method: "GET",
            dataType: "json",
            url: url,
            //data: data,
            success: function(data) {
              $("#timeelapsed").text(data.took);
              $("#searchresultcount").text(data.hits.total);
   
              var counter = 0;

              var items = [];
              $.each( data.hits.hits, function( key, val ) {
                items.push( "<article class='search-result row'><div class='col-xs-12 col-sm-12 col-md-12'><h3><a href='#' title=''>" + val._source.db + " " + val._source.level +"</a></h3><p>" + val._source.message + "</p></div></article>" );
              });

              $("#searchresults").html(items.join(""));


            },
            fail: function() {
              alert( "fail" );
            }

          });

          // var jqxhr = $.getJSON(, 
          // function(data) {
          //   alert( "success" );
          //   alert(data.took);
          //   alert(data.hits.total);
          // })
          //   .done(function() {
          //     alert( "second success" );
          //   })
          //   .fail(function() {
          //     alert( "error" );
          //   })
          //   .always(function() {
          //     alert( "finished" );
          //   });
           
          // // Perform other work here ...
           
          // // Set another completion function for the request above
          // jqxhr.always(function() {
          //   alert( "second finished" );
          // });          
        }, 600 );
    });
})


var delay = (function(){
  var timer = 0;
  return function(callback, ms){
    clearTimeout (timer);
    timer = setTimeout(callback, ms);
  };
})();