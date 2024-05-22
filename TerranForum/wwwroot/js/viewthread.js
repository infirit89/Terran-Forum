const uri = "../Post/GetCreatePostReplyView";
let previousCreateReplyContainer = null;

(function setup() {
	const createReplyContainers = document.querySelectorAll('#createReply');
	const parser = new DOMParser();

	const addReplyButtons = document.querySelectorAll('#addReply');
	for (const addReplyButton of addReplyButtons) {
		addReplyButton.addEventListener('click', e => {
			const createReplyContainer = addReplyButton.parentElement;
			const postId = addReplyButton.getAttribute("postId");
			const forumId = addReplyButton.getAttribute("forumId");

			fetch(`${uri}?forumId=${forumId}&postId=${postId}`)
				.then(response => response.text())
				.then(responseText => {
					const parsedDom = parser.parseFromString(responseText, "text/html");
					createReplyContainer.appendChild(parsedDom.querySelector('div'));

					const cancelReplyButton = createReplyContainer
						.lastElementChild
						.querySelector('#cancelReply');

					cancelReplyButton.addEventListener('click', e => {
						createReplyContainer.removeChild(createReplyContainer.lastChild);
					});

					if (previousCreateReplyContainer != null && previousCreateReplyContainer != createReplyContainer)
						previousCreateReplyContainer.removeChild(previousCreateReplyContainer.lastChild);

					previousCreateReplyContainer = createReplyContainer;
				})
				.catch(error => console.error(error));
		});
	}
})();
