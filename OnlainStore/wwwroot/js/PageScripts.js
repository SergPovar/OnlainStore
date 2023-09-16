$(function (){
    $("body").on("click","a.delete",function (){
        if (!confirm("Confirm page deletion")) return false;
    });
    $("#pages tbody").sortable(   {
        items : "tr:not(.home)",
        placeholder : "ui-state-highlight",
        update : function (){
            let ids = $("#pages tbody").sortable("serialize");
            console.log(ids);
            let url = "/Admin/Shop/ReorderCategories";
            $.post(url,  ids, function(data) {
            });
        }
    });
});
