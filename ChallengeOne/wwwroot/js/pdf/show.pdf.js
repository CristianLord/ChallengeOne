document.addEventListener('DOMContentLoaded', function () {

    // atob() is used to convert base64 encoded PDF to binary-like data.
    // (See also https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/
    // Base64_encoding_and_decoding.)
    var pdfData = '~/Archivo.pdf'

    //
    // The workerSrc property shall be specified.
    //https://cdn.jsdelivr.net/npm/pdfjs-dist@2.4.456/build/pdf.worker.min.js
    pdfjsLib.GlobalWorkerOptions.workerSrc =
        '~/js/pdf/pdf.worker.min.js';

    var pdfDoc = null,
        pageNum = 1,
        pageRendering = false,
        pageNumPending = null,
        scale = 1.5,
        canvas = document.getElementById('the-canvas')
    ctx = canvas.getContext('2d');

    function renderPage(num) {
        pageRendering = true
        pdfDoc.getPage(num).then((page) => {
            var viewport = page.getViewport({ scale: scale });
            canvas.height = viewport.height
            canvas.width = viewport.width

            var renderContext = {
                canvasContext: ctx,
                viewport: viewport
            }
            var renderTask = page.render(renderContext)
            renderTask.promise.then(() => {
                pageRendering = false;
                if (pageNumPending !== null) {
                    renderPage(pageNumPending)
                    pageNumPending = null
                }
            })
        })
        //document.getElementById('page_num').textContent = num;
    }

    function queueRenderPage(num) {
        if (pageRendering) {
            pageNumPending = num
        } else {
            renderPage(num)
        }
    }

    function onPrevPage() {
        if (pageNum <= 1) {
            return
        }
        pageNum--;
        queueRenderPage(pageNum)
    }
    //document.getElementById('prev').addEventListener('click', onPrevPage)

    function onNextPage() {
        if (pageNum >= pdfDoc.numPages) {
            return
        }
        pageNum++;
        queueRenderPage(pageNum)
    }
    //document.getElementById('next').addEventListener('click', onNextPage)

    function zoomIn() {
        scale += 0.1
        renderPage(pageNum)
    }
    function zoomOut() {
        scale + - 0.1
        renderPage(pageNum)
    }
    // Opening PDF by passing its binary data as a string. It is still preferable
    // to use Uint8Array, but string or array-like structure will work too.
    var loadingTask = pdfjsLib.getDocument({ url: '~/Archivo.pdf', });
    loadingTask.promise.then(function (pdf) {
        pdfDoc = pdf
        document.getElementById('page_count').textContent = pdf.numPages;
        renderPage(pageNum)
    });
});