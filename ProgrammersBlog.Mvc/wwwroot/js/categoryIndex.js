﻿$(document).ready(function () {
    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: { id: 'btnAdd' },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {

                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'Get',
                        url: '/Admin/Category/GetAllCategories/',
                        contentType: "applicatio/json",
                        beforeSend: function () {
                            $('#categoriesTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            console.log(data);
                            const categoryListDto = jQuery.parseJSON(data);
                            console.log(categoryListDto);
                            if (categoryListDto.ResultStatus === 0) {
                                let tableBody = '';
                                $.each(categoryListDto.Categories.$values, function (index, category) {

                                    console.log(category);
                                    tableBody += `<tr>
                                                                        <td>${category.Id}</td>
                                                                        <td>${category.Name}</td>
                                                                        <td>${category.Description}</td>
                                                                        <td>${convertFirstLetterToUpperCase(category.IsActive.toString())}</td>
                                                                        <td>${convertFirstLetterToUpperCase(category.IsDeleted.toString())}</td>
                                                                        <td>${category.Note}</td>
                                                                        <td>${convertToShortDate(category.CreatedDate)}</td>
                                                                        <td>${category.CreatedByName}</td>
                                                                        <td>${convertToShortDate(category.ModifiedDate)}</td>
                                                                        <td>${category.ModifiedByName}</td>
                                                                        <td>
                                            <button class="btn btn-primary btn-sm"><span class="fas fa-edit"></span></button>
                                            <button class="btn btn-danger btn-sm btn-delete" data-id="${category.Id}"><span class="fas fa-minus-circle"></span></button>
                                        </td>

                                                                    </tr>`

                                });
                                $('#categoriesTable > tbody').replaceWith(tableBody);
                                $('.spinner-border').hide();
                                $('#categoriesTable').fadeIn(1400);
                            } else {

                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#categoriesTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Hata');
                        }
                    });
                }
            }
        ],
        language: {
            "emptyTable": "Tabloda herhangi bir veri mevcut değil",
            "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "infoEmpty": "Kayıt yok",
            "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "infoThousands": ".",
            "lengthMenu": "Sayfada _MENU_ kayıt göster",
            "loadingRecords": "Yükleniyor...",
            "processing": "İşleniyor...",
            "search": "Ara:",
            "zeroRecords": "Eşleşen kayıt bulunamadı",
            "paginate": {
                "first": "İlk",
                "last": "Son",
                "next": "Sonraki",
                "previous": "Önceki"
            },
            "aria": {
                "sortAscending": ": artan sütun sıralamasını aktifleştir",
                "sortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "1": "1 kayıt seçildi"
                },
                "cells": {
                    "1": "1 hücre seçildi",
                    "_": "%d hücre seçildi"
                },
                "columns": {
                    "1": "1 sütun seçildi",
                    "_": "%d sütun seçildi"
                }
            },
            "autoFill": {
                "cancel": "İptal",
                "fillHorizontal": "Hücreleri yatay olarak doldur",
                "fillVertical": "Hücreleri dikey olarak doldur",
                "fill": "Bütün hücreleri <i>%d<\/i> ile doldur"
            },
            "buttons": {
                "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
                "colvis": "Sütun görünürlüğü",
                "colvisRestore": "Görünürlüğü eski haline getir",
                "copySuccess": {
                    "1": "1 satır panoya kopyalandı",
                    "_": "%ds satır panoya kopyalandı"
                },
                "copyTitle": "Panoya kopyala",
                "csv": "CSV",
                "excel": "Excel",
                "pageLength": {
                    "-1": "Bütün satırları göster",
                    "_": "%d satır göster"
                },
                "pdf": "PDF",
                "print": "Yazdır",
                "copy": "Kopyala",
                "copyKeys": "Tablodaki veriyi kopyalamak için CTRL veya u2318 + C tuşlarına basınız. İptal etmek için bu mesaja tıklayın veya escape tuşuna basın.",
                "createState": "Şuanki Görünümü Kaydet",
                "removeAllStates": "Tüm Görünümleri Sil",
                "removeState": "Aktif Görünümü Sil",
                "renameState": "Aktif Görünümün Adını Değiştir",
                "savedStates": "Kaydedilmiş Görünümler",
                "stateRestore": "Görünüm -&gt; %d",
                "updateState": "Aktif Görünümün Güncelle"
            },
            "searchBuilder": {
                "add": "Koşul Ekle",
                "button": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "condition": "Koşul",
                "conditions": {
                    "date": {
                        "after": "Sonra",
                        "before": "Önce",
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "number": {
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "gt": "Büyüktür",
                        "gte": "Büyük eşittir",
                        "lt": "Küçüktür",
                        "lte": "Küçük eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "string": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "endsWith": "İle biter",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "startsWith": "İle başlar",
                        "notContains": "İçermeyen",
                        "notStartsWith": "Başlamayan",
                        "notEndsWith": "Bitmeyen"
                    },
                    "array": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "without": "Hariç"
                    }
                },
                "data": "Veri",
                "deleteTitle": "Filtreleme kuralını silin",
                "leftTitle": "Kriteri dışarı çıkart",
                "logicAnd": "ve",
                "logicOr": "veya",
                "rightTitle": "Kriteri içeri al",
                "title": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "value": "Değer",
                "clearAll": "Filtreleri Temizle"
            },
            "searchPanes": {
                "clearMessage": "Hepsini Temizle",
                "collapse": {
                    "0": "Arama Bölmesi",
                    "_": "Arama Bölmesi (%d)"
                },
                "count": "{total}",
                "countFiltered": "{shown}\/{total}",
                "emptyPanes": "Arama Bölmesi yok",
                "loadMessage": "Arama Bölmeleri yükleniyor ...",
                "title": "Etkin filtreler - %d",
                "showMessage": "Tümünü Göster",
                "collapseMessage": "Tümünü Gizle"
            },
            "thousands": ".",
            "datetime": {
                "amPm": [
                    "öö",
                    "ös"
                ],
                "hours": "Saat",
                "minutes": "Dakika",
                "next": "Sonraki",
                "previous": "Önceki",
                "seconds": "Saniye",
                "unknown": "Bilinmeyen",
                "weekdays": {
                    "6": "Paz",
                    "5": "Cmt",
                    "4": "Cum",
                    "3": "Per",
                    "2": "Çar",
                    "1": "Sal",
                    "0": "Pzt"
                },
                "months": {
                    "9": "Ekim",
                    "8": "Eylül",
                    "7": "Ağustos",
                    "6": "Temmuz",
                    "5": "Haziran",
                    "4": "Mayıs",
                    "3": "Nisan",
                    "2": "Mart",
                    "11": "Aralık",
                    "10": "Kasım",
                    "1": "Şubat",
                    "0": "Ocak"
                }
            },
            "decimal": ",",
            "editor": {
                "close": "Kapat",
                "create": {
                    "button": "Yeni",
                    "submit": "Kaydet",
                    "title": "Yeni kayıt oluştur"
                },
                "edit": {
                    "button": "Düzenle",
                    "submit": "Güncelle",
                    "title": "Kaydı düzenle"
                },
                "error": {
                    "system": "Bir sistem hatası oluştu (Ayrıntılı bilgi)"
                },
                "multi": {
                    "info": "Seçili kayıtlar bu alanda farklı değerler içeriyor. Seçili kayıtların hepsinde bu alana aynı değeri atamak için buraya tıklayın; aksi halde her kayıt bu alanda kendi değerini koruyacak.",
                    "noMulti": "Bu alan bir grup olarak değil ancak tekil olarak düzenlenebilir.",
                    "restore": "Değişiklikleri geri al",
                    "title": "Çoklu değer"
                },
                "remove": {
                    "button": "Sil",
                    "confirm": {
                        "_": "%d adet kaydı silmek istediğinize emin misiniz?",
                        "1": "Bu kaydı silmek istediğinizden emin misiniz?"
                    },
                    "submit": "Sil",
                    "title": "Kayıtları sil"
                }
            },
            "stateRestore": {
                "creationModal": {
                    "button": "Kaydet",
                    "columns": {
                        "search": "Kolon Araması",
                        "visible": "Kolon Görünümü"
                    },
                    "name": "Görünüm İsmi",
                    "order": "Sıralama",
                    "paging": "Sayfalama",
                    "scroller": "Kaydırma (Scrool)",
                    "search": "Arama",
                    "searchBuilder": "Arama Oluşturucu",
                    "select": "Seçimler",
                    "title": "Yeni Görünüm Oluştur",
                    "toggleLabel": "Kaydedilecek Olanlar"
                },
                "duplicateError": "Bu Görünüm Daha Önce Tanımlanmış",
                "emptyError": "Görünüm Boş Olamaz",
                "emptyStates": "Herhangi Bir Görünüm Yok",
                "removeJoiner": "ve",
                "removeSubmit": "Sil",
                "removeTitle": "Görünüm Sil",
                "renameButton": "Değiştir",
                "renameLabel": "Görünüme Yeni İsim Ver -&gt; %s:",
                "renameTitle": "Görünüm İsmini Değiştir",
                "removeConfirm": "Görünümü silmek istediğinize emin misiniz?",
                "removeError": "Görünüm silinemedi"
            }
        }
    });
    /* DataTable burada bitiyor */
        /* _CategoryAddPartialView modal olarak çağırılıyor */
    $(function () {

        const url = '/Admin/Category/Add/';
        const placeHolderDiv = $('#modalPlaceHolder');

        $('#btnAdd').click(function () {

            $.get(url).done(function (data) {
                placeHolderDiv.html(data);

                placeHolderDiv.find(".modal").modal('show');
            });
        });
        /* _CategoryAddPartialView modal olarak çağırılıyor burada bitti */
        /* _CategoryAddPartialView modal olarak çağırılıyor */
        placeHolderDiv.on('click', '#btnSave', function (event) {
            event.preventDefault();
            const form = $('#form-category-add');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const categoryAddAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', categoryAddAjaxModel.CategoryAddPartial);
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    const newTableRow = `<tr>
                                                                                                <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                                                                                                <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                                                                                                <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                                                                                                <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsActive.toString())}</td>
                                                                                                <td>${convertFirstLetterToUpperCase(categoryAddAjaxModel.CategoryDto.Category.IsDeleted.toString())}</td>
                                                                                                <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                                                                                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                                                                                <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                                                                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                                                                                <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
  <td>
                                            <button class="btn btn-primary btn-sm"><span class="fas fa-edit"></span></button>
                                            <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                                        </td>
                                                                                                </tr>`;
                    const newTableRowObject = $(newTableRow);
                    newTableRowObject.hide();
                    $('#categoriesTable').append(newTableRowObject);
                    newTableRowObject.fadeIn(3500);
                    toastr.success(`${categoryAddAjaxModel.CategoryDto.Category.Message}`, 'Başarılı İşlem');

                } else {
                    let summaryText = '';
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });

                    toastr.warning(summaryText);
                }
            });
        });
    });
    //Ajax post/ categoryadddto post işlemi burada bitiyor
    //ajax post category silme işlemi burada başlıyor
    $(document).on('click', '.btn-delete', function (event) {
        event.preventDefault();
        const id = $(this).attr('data-id');
        const tableRow = $(`[name="${id}"]`);
        const categoryName = tableRow.find('td:eq(1)').text();
        console.log(id);
        Swal.fire({
            title: 'Silmek istediğinize emin misiniz ?',
            text: `${categoryName} adlı kategori silinecektir.`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek istiyorum!',
            cancelButtonText: 'Hayır, silmek istemiyorum'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    data: { categoryId: id },
                    url: '/Admin/Category/Delete/',
                    success: function (data) {
                        const categoryDto = jQuery.parseJSON(data);
                        if (categoryDto.ResultStatus === 0) {
                            console.log("result status 0 a eşit");
                            Swal.fire(
                                'Silindi!',
                                `${categoryDto.Category.Name} adlı kategori başarıyla silinmiştir.`,
                                'success'
                            );

                            tableRow.fadeOut(3500);
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Başarısız İşlem',
                                text: `${categoryDto.Message}`

                            });
                        }
                    },
                    error: function (err) {
                        console.log(err);
                        toastr.error(`${err.responseText}`, 'Hata!');
                    }

                })
            }
        });
    });
});