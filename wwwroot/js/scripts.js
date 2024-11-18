
document.getElementById('shortenForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const originalUrl = document.getElementById('originalUrl').value.trim();
    const customAlias = document.getElementById('customAlias').value.trim() || null;
    const shortUrlOutput = document.getElementById('shortUrl');
    // Reset previous output
    shortUrlOutput.innerHTML = '';
    try {
        if (customAlias && customAlias.length !== 5) {
            throw new Error('Custom alias must be exactly 5 characters long.');
        }
        const response = await fetch('/api/url', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ originalUrl, customAlias })
        });
        if (!response.ok) {
            throw new Error('Failed to shorten URL. Please try again.');
        }
        const data = await response.json();
        shortUrlOutput.innerHTML = `Short URL: <a href="${data.shortUrl}" target="_blank" class="short-url-link">${data.shortUrl}</a>`;
        loadHistory(); // Reload history after shortening a URL
    } catch (error) {
        shortUrlOutput.innerHTML = `<span style="color: #e74c3c;">${error.message}</span>`;
    }
});

document.getElementById('deleteHistoryButton').addEventListener('click', async () => {
    try {
        const response = await fetch('/api/url/history', {
            method: 'DELETE'
        });
        if (!response.ok) {
            throw new Error('Failed to delete history. Please try again.');
        }
        loadHistory(); // Reload history after deletion
    } catch (error) {
        alert(error.message);
    }
});

async function loadHistory() {
    const historyList = document.getElementById('historyList');
    historyList.innerHTML = ''; // Clear previous history
    try {
        const response = await fetch('/api/url/history');
        if (!response.ok) {
            throw new Error('Failed to load history. Please try again.');
        }
        const data = await response.json();
        data.forEach(item => {
            const listItem = document.createElement('li');
            const logoUrl = `https://logo.clearbit.com/${new URL(item.originalUrl).hostname}`;
            const fallbackLogoUrl = `https://icons.duckduckgo.com/ip3/${new URL(item.originalUrl).hostname}.ico`;
            const fullShortUrl = `${window.location.origin}/api/url/${item.shortUrl}`;
            const createdAt = new Date(item.createdAt).toLocaleString();
            listItem.innerHTML = `
                <img src="${logoUrl}" alt="Logo" class="logo" onerror="this.onerror=null;this.src='${fallbackLogoUrl}';">
                <div class="url-info">
                    <span class="original-url">Original URL: <a href="${item.originalUrl}" target="_blank">${item.originalUrl}</a></span>
                    <span class="short-url">Short URL: <a href="${fullShortUrl}" target="_blank">${fullShortUrl}</a></span>
                    <span class="created-at">Created At: ${createdAt}</span>
                </div>
            `;
            historyList.appendChild(listItem);
        });
    } catch (error) {
        historyList.innerHTML = `<span style="color: #e74c3c;">${error.message}</span>`;
    }
}

// Load history on page load
loadHistory();