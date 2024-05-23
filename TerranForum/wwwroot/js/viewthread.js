(function setupRatingButtons() {
	const ratingContainers = document.querySelectorAll('#ratingContainer');

	for (const ratingContainer of ratingContainers) {
		const likeButton = ratingContainer.querySelector('#like');
		const dislikeButton = ratingContainer.querySelector('#dislike');
		
		const data = {
			postId: ratingContainer.getAttribute('postId'),
			rating: 0
		};
		
		likeButton.addEventListener('click', (e) => {
			e.preventDefault();

		});

		dislikeButton.addEventListener('click', (e) => {
			e.preventDefault();
		});
	}
})();

(function setupCommentForms() {
	const parser = new DOMParser();
	let previousCreateReplyContainer = null;

	const addReplyButtons = document.querySelectorAll('#addReply');
	for (const addReplyButton of addReplyButtons) {
		addReplyButton.addEventListener('click', e => {
			e.preventDefault();
			const createReplyContainer = addReplyButton.parentElement;
			const postId = addReplyButton.getAttribute("postId");
			const forumId = addReplyButton.getAttribute("forumId");

			fetch(`${addReplyButton.href}?forumId=${forumId}&postId=${postId}`)
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

					const form = createReplyContainer
						.lastElementChild.querySelector('form');

					form.addEventListener('submit', (e) => {
						e.preventDefault();

						// because the server sends the partial view;
						// the validation needs to be called manually
						// kinda ass tbh
						$.validator.unobtrusive.parse(form);
						if ($(form).valid())
							form.submit();
					});
					
					if (previousCreateReplyContainer != null && previousCreateReplyContainer != createReplyContainer)
						previousCreateReplyContainer.removeChild(previousCreateReplyContainer.lastChild);

					previousCreateReplyContainer = createReplyContainer;
				})
				.catch(error => console.error(error));
		});
	}
})();
