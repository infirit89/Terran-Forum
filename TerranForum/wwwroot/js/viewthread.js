(function setupManageButtons()
{
	const manageContainers = document.querySelectorAll('#managePost');
	for (const manageContainer of manageContainers)
	{
		const deleteButton = manageContainer.querySelector('a');
		const postId = manageContainer.getAttribute('postId');
		deleteButton.addEventListener('click', (e) => {
			e.preventDefault();
		});
	}
})();

(function setupRatingButtons() {
	const ratingModifyContainers = document.querySelectorAll('#ratingModifyContainer');

	for (const ratingModifyContainer of ratingModifyContainers) {
		const likeButton = ratingModifyContainer.querySelector('#like');
		const dislikeButton = ratingModifyContainer.querySelector('#dislike');

		const postId = ratingModifyContainer.getAttribute('postId');

		function updateRating(uri, ratingModifier, ratingContainer) {
			fetch(`${uri}?postId=${postId}&rating=${ratingModifier}`, {
				method: 'PUT'
			})
			.then(response => response.json())
			.then(data =>
				ratingContainer.textContent = Number(data.rating))
			.catch(error => console.error(error));
		}

		likeButton.addEventListener('click', (e) => {
			e.preventDefault();
			const ratingModifier = 1;
			const ratingContainer = ratingModifyContainer.querySelector('div');
			
			updateRating(likeButton.href, ratingModifier, ratingContainer);
		});

		dislikeButton.addEventListener('click', (e) => {
			e.preventDefault();
			const ratingModifier = -1;
			const ratingContainer = ratingModifyContainer.querySelector('div');
			
			updateRating(dislikeButton.href, ratingModifier, ratingContainer);
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
