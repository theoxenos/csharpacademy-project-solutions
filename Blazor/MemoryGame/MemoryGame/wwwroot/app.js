window.memoryGame = {
    scrollIntoViewIfNeeded: (element) => {
        if (!element) {
            return;
        }

        element.scrollIntoView({
            behavior: "smooth",
            block: "start"
        });
    }
};
