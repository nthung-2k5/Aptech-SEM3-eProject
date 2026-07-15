// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Đặt trong _Layout.cshtml (trước thẻ </body>) hoặc trong site.js
document.addEventListener('DOMContentLoaded', function () {
    
    // Lắng nghe sự kiện submit của toàn bộ trang web
    document.addEventListener('submit', function (e) {
        const form = e.target;

        // 1. Nếu form bị lỗi Validate HTML5 (ví dụ chưa nhập email), trình duyệt sẽ tự chặn, ta không cần hiện loading.
        if (!form.checkValidity()) {
            return; 
        }

        // 2. Bỏ qua nếu Form có class 'no-loading' (Dành cho các form bạn KHÔNG muốn dùng hiệu ứng này)
        if (form.classList.contains('no-loading')) {
            return;
        }

        // 3. Tìm nút Submit nằm trong cái Form vừa bị bấm
        const submitBtn = form.querySelector('button[type="submit"]');
        
        if (submitBtn) {
            // Tránh trường hợp user dùng tool click liên tục
            if (submitBtn.dataset.processing === "true") return;
            submitBtn.dataset.processing = "true";

            // 4. Lấy chiều rộng hiện tại của nút để giữ form không bị giật/co giãn khi thay đổi text
            const currentWidth = submitBtn.offsetWidth;
            submitBtn.style.width = currentWidth + 'px';

            // 5. Thêm vòng xoay DaisyUI và đổi chữ tự động
            submitBtn.innerHTML = `<span class="loading loading-spinner"></span> Processing...`;

            // 6. Khóa nút sau 15 mili-giây để form kịp bay về C#
            setTimeout(function () {
                submitBtn.disabled = true;
            }, 15);
        }
    });

});