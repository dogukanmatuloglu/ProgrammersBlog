﻿

$(document).ready(function () {
    //Trumbowyg
    $('#text-editor').trumbowyg({
        btns: [
            ['viewHTML'],
            ['undo', 'redo'], // Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['fullscreen'],
            ['foreColor', 'backColor'],
            ['emoji'],
            ['fontfamily'],
            ['fontsize']
        ]
    });
    //Trumbowyg

    //Select2

    $('#categoryList').select2({
        theme: 'bootstrap4',
        placeholder: "Lütfen Bir Kategori Seçiniz..",
        allowClear: true
    });
});
