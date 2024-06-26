﻿(function setupManageButtons()
{
	const parser = new DOMParser();
	const manageContainers = document.querySelectorAll('#managePost');
	for (const manageContainer of manageContainers) {
		const deleteButton = manageContainer.querySelector('#deleteBtn');
		const forumId = manageContainer.getAttribute('forumId');
		const postId = manageContainer.getAttribute('postId');
		deleteButton.addEventListener('click', (e) => {
			e.preventDefault();

			fetch(`${deleteButton.href}?forumId=${forumId}&postId=${postId}`)
				.then(response => response.status == 200 ? response.text() : '')
				.then((responseText) =>
				{
					const mainContainer = document.querySelector('main');
					const parsedDom = parser.parseFromString(responseText, "text/html");
					mainContainer.appendChild(parsedDom.querySelector('div'));
					const deleteModal = mainContainer.querySelector('#deleteModal');
					const modal = new bootstrap.Modal(deleteModal);

					deleteModal.querySelector('#dimissBtn').addEventListener('click', () => {
						modal.hide();
						mainContainer.removeChild(deleteModal);
					});

					modal.toggle();
				})
				.catch(error => console.error(error));
		});
	}

	const replyManageContainers = document.querySelectorAll('#manageReply');
	for (const manageContainer of replyManageContainers) {
		const postId = manageContainer.getAttribute('postId');
		const replyId = manageContainer.getAttribute('replyId');
		
		const deleteButton = manageContainer.querySelector('a');
		if (deleteButton == null)
			continue;

		deleteButton.addEventListener('click', (e) => {
			e.preventDefault();

			fetch(`${deleteButton.href}?postId=${postId}&replyId=${replyId}`)
				.then(response => response.status == 200 ? response.text() : '')
				.then((responseText) => {
					const mainContainer = document.querySelector('main');
					const parsedDom = parser.parseFromString(responseText, "text/html");
					console.log(parsedDom);
					mainContainer.appendChild(parsedDom.querySelector('div'));
					const deleteModal = mainContainer.querySelector('#deleteModal');
					const modal = new bootstrap.Modal(deleteModal);

					deleteModal.querySelector('#dimissBtn').addEventListener('click', () => {
						modal.hide();
						mainContainer.removeChild(deleteModal);
					});

					modal.toggle();
				})
				.catch(error => console.error(error));
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

		if (likeButton === null || dislikeButton === null)
			continue;

		likeButton.addEventListener('click', (e) => {
			e.preventDefault();
			const ratingModifier = 1;
			const ratingContainer = ratingModifyContainer.querySelector('div');

			upvoteIconStyle = likeButton.children[0].style;
			downvoteIconStyle = dislikeButton.children[0].style;

			upvoteFontVariationSettingsStr = new String(upvoteIconStyle.fontVariationSettings);
			downvoteFontVariationSettingsStr = new String(downvoteIconStyle.fontVariationSettings);
			
			if (upvoteFontVariationSettingsStr.includes('"FILL" 0')
				|| downvoteFontVariationSettingsStr.includes('"FILL" 1')) {
				upvoteIconStyle.fontVariationSettings = upvoteFontVariationSettingsStr.replace('"FILL" 0', '"FILL" 1');
				downvoteIconStyle.fontVariationSettings = downvoteFontVariationSettingsStr.replace('"FILL" 1', '"FILL" 0');
			}
			else if (upvoteFontVariationSettingsStr.includes('"FILL" 1')) {
				upvoteIconStyle.fontVariationSettings = upvoteFontVariationSettingsStr.replace('"FILL" 1', '"FILL" 0');
			}
			
			updateRating(likeButton.href, ratingModifier, ratingContainer);
		});

		dislikeButton.addEventListener('click', (e) => {
			e.preventDefault();
			const ratingModifier = -1;
			const ratingContainer = ratingModifyContainer.querySelector('div');

			upvoteIconStyle = likeButton.children[0].style;
			downvoteIconStyle = dislikeButton.children[0].style;

			upvoteFontVariationSettingsStr = new String(upvoteIconStyle.fontVariationSettings);
			downvoteFontVariationSettingsStr = new String(downvoteIconStyle.fontVariationSettings);

			if (downvoteFontVariationSettingsStr.includes('"FILL" 0')
				|| upvoteFontVariationSettingsStr.includes('"FILL" 1')) {
				downvoteIconStyle.fontVariationSettings = downvoteFontVariationSettingsStr.replace('"FILL" 0', '"FILL" 1');
				upvoteIconStyle.fontVariationSettings = upvoteFontVariationSettingsStr.replace('"FILL" 1', '"FILL" 0');
			}
			else if (downvoteFontVariationSettingsStr.includes('"FILL" 1')) {
				downvoteIconStyle.fontVariationSettings = downvoteFontVariationSettingsStr.replace('"FILL" 1', '"FILL" 0');
			}

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
