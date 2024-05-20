(function setup() {
	const invisibleClass = 'invisible';
	const addReplyButtons = document.querySelectorAll('#addReply');
	for (const addReplyButton of addReplyButtons) {
		addReplyButton.addEventListener('click', e => {
			const replyContainer = addReplyButton.parentElement.querySelector('div');

			if (replyContainer.classList.contains(invisibleClass))
				replyContainer.classList.remove(invisibleClass);

			if(!addReplyButton.classList.contains(invisibleClass))
				addReplyButton.classList.add(invisibleClass);
		});
	}

	const cancelReplyButtons = document.querySelectorAll('#cancelReply');
	for (const cancelReplyButton of cancelReplyButtons) {
		cancelReplyButton.addEventListener('click', e => {
			const replyContainer = cancelReplyButton.parentElement.parentElement;
			const addReplyButton = replyContainer.parentElement.querySelector('#addReply');

			if (!replyContainer.classList.contains(invisibleClass))
				replyContainer.classList.add(invisibleClass);

			if (addReplyButton.classList.contains(invisibleClass))
				addReplyButton.classList.remove(invisibleClass);
		});
	}
})();
