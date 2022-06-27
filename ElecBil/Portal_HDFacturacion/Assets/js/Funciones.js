

var estado_ajax = false;
function CargarForm(module, frm, title) {
    document.title = 'Plantilla | ' + title;
    try {
        $(".content").html("");
        if (estado_ajax == false) {
            estado_ajax = true;
            //if (xhr_p && xhr_p.readyState != 4) {
            //    xhr_p.abort();
            //}

            //if ($('.overlay').is(":visible")) {
            //    //alert('Elemento visible');
            //    $(".bars").click();
            //    $(".overlay").click();
            //}
            $(".content").css({ "opacity": "1" });
            $(".content").stop();
            //$(".content").html("").hide("fade");

            $(".content").hide("fade");
            var form = '/' + module + '/' + frm + '';
            alert(form);
            setTimeout(function () {
                //xhr_p =
                $.ajax({
                    url: form,
                    type: 'post',
                    beforeSend: function () {

                    },
                    success: function (response) {
                        $('.content').html(response);
                        $('.content').show("fade");
                        estado_ajax = false;
                        //console.log("Bien");
                    }, error: function (jqxhr, textStatus, errorThrown) {
                        $(".content").html("");
                        estado_ajax = false;

                        console.log("mal");
                    }
                });
            }, 500);

        }

    } catch (ex) {
        alert(ex);
        estado_ajax = false;
    }
    return false;
}







