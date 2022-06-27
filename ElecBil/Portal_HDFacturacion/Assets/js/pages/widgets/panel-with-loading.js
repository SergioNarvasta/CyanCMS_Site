(function ($) {
    'use strict';
    $(function () {

        $('[data-panel-loading="true"]').on('click', function () {
            var effect = $(this).data('loadingEffect');
            var color = $(this).data('loadingColor');
            if (color === undefined) color = '#555';

            var $loading = $(this).parents('.theme-green').waitMe({ //panel
                effect: effect,
                text: 'Espere un momento...',
                bg: 'rgba(255,255,255,0.90)',
                color: color
            });

            setTimeout(function () {
                //Loading hide
                $loading.waitMe('hide');
            }, 3200);
        });

    });
}(jQuery))
