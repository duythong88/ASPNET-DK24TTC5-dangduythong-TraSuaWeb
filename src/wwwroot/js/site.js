// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<script>
    document.addEventListener("DOMContentLoaded", function () {

        let maSp = '@Context.Session.GetInt32("SanPhamDangChon")';

    if (maSp) {
        moModalChonSize(maSp);

    // clear sau khi dùng
    fetch('/SanPham/XoaSession');
    }
});
</script>