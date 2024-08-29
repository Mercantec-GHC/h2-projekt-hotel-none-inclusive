class StickyNavigation {
    constructor() {
        this.currentId = null;
        this.currentTab = null;
        this.tabContainerHeight = 70;

        // Initialize event listeners
        this.initEventListeners();
    }

    initEventListeners() {
        // Event listeners for tab clicks (if needed for sticky navigation)
        document.querySelectorAll('.et-hero-tab').forEach(tab => {
            tab.addEventListener('click', (event) => this.onTabClick(event, tab));
        });

        // Window scroll and resize event listeners for sticky navigation
        window.addEventListener('scroll', () => this.onScroll());
        window.addEventListener('resize', () => this.onResize());

        // Optional: Ensure Bootstrap dropdowns are initialized
        this.initBootstrapDropdowns();
    }

    onTabClick(event, element) {
        event.preventDefault();
        let scrollTop = document.querySelector(element.getAttribute('href')).offsetTop - this.tabContainerHeight + 1;
        window.scrollTo({ top: scrollTop, behavior: 'smooth' });
    }

    onScroll() {
        this.checkTabContainerPosition();
        this.findCurrentTabSelector();
    }

    onResize() {
        if (this.currentId) {
            this.setSliderCss();
        }
    }

    checkTabContainerPosition() {
        let offset = document.querySelector('.et-hero-tabs').offsetTop + document.querySelector('.et-hero-tabs').offsetHeight - this.tabContainerHeight;
        if (window.scrollY > offset) {
            document.querySelector('.et-hero-tabs-container').classList.add('et-hero-tabs-container--top');
        } else {
            document.querySelector('.et-hero-tabs-container').classList.remove('et-hero-tabs-container--top');
        }
    }

    findCurrentTabSelector() {
        let newCurrentId;
        let newCurrentTab;
        document.querySelectorAll('.et-hero-tab').forEach(tab => {
            let id = tab.getAttribute('href');
            let offsetTop = document.querySelector(id).offsetTop - this.tabContainerHeight;
            let offsetBottom = document.querySelector(id).offsetTop + document.querySelector(id).offsetHeight - this.tabContainerHeight;
            if (window.scrollY > offsetTop && window.scrollY < offsetBottom) {
                newCurrentId = id;
                newCurrentTab = tab;
            }
        });
        if (this.currentId !== newCurrentId || this.currentId === null) {
            this.currentId = newCurrentId;
            this.currentTab = newCurrentTab;
            this.setSliderCss();
        }
    }

    setSliderCss() {
        let width = 0;
        let left = 0;
        if (this.currentTab) {
            width = this.currentTab.offsetWidth + 'px';
            left = this.currentTab.offsetLeft + 'px';
        }
        let slider = document.querySelector('.et-hero-tab-slider');
        if (slider) {
            slider.style.width = width;
            slider.style.left = left;
        }
    }

    initBootstrapDropdowns() {
        // Bootstrap dropdowns may not need manual initialization as they're handled by Bootstrap's own JS
        // But this ensures dropdown functionality is active if somehow not
        document.querySelectorAll('.dropdown-toggle').forEach((element) => {
            new bootstrap.Dropdown(element);
        });
    }
}

// Ensure StickyNavigation does not interfere with Bootstrap dropdown
document.addEventListener('DOMContentLoaded', () => new StickyNavigation());
