class StickyNavigation {
    constructor() {
        this.currentId = null;
        this.currentTab = null;
        this.tabContainerHeight = 70;

        
        this.initEventListeners();
    }

    initEventListeners() {
        
        const tabs = document.querySelectorAll('.et-hero-tab');
        if (tabs.length) {
            tabs.forEach(tab => {
                tab.addEventListener('click', (event) => this.onTabClick(event, tab));
            });
        }

        
        window.addEventListener('scroll', this.debounce(() => this.onScroll(), 50));
        window.addEventListener('resize', this.debounce(() => this.onResize(), 50));

        
        this.initBootstrapDropdowns();
    }

    onTabClick(event, element) {
        event.preventDefault();
        const targetElement = document.querySelector(element.getAttribute('href'));
        if (targetElement) {
            const scrollTop = targetElement.offsetTop - this.tabContainerHeight + 1;
            window.scrollTo({ top: scrollTop, behavior: 'smooth' });
        }
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
        const tabs = document.querySelector('.et-hero-tabs');
        const tabsContainer = document.querySelector('.et-hero-tabs-container');
        if (tabs && tabsContainer) {
            const offset = tabs.offsetTop + tabs.offsetHeight - this.tabContainerHeight;
            if (window.scrollY > offset) {
                tabsContainer.classList.add('et-hero-tabs-container--top');
            } else {
                tabsContainer.classList.remove('et-hero-tabs-container--top');
            }
        }
    }

    findCurrentTabSelector() {
        let newCurrentId;
        let newCurrentTab;

        document.querySelectorAll('.et-hero-tab').forEach(tab => {
            const id = tab.getAttribute('href');
            const targetElement = document.querySelector(id);

            if (targetElement) {
                const offsetTop = targetElement.offsetTop - this.tabContainerHeight;
                const offsetBottom = offsetTop + targetElement.offsetHeight;

                if (window.scrollY > offsetTop && window.scrollY < offsetBottom) {
                    newCurrentId = id;
                    newCurrentTab = tab;
                }
            }
        });

        if (this.currentId !== newCurrentId || this.currentId === null) {
            this.currentId = newCurrentId;
            this.currentTab = newCurrentTab;
            this.setSliderCss();
        }
    }

    setSliderCss() {
        const slider = document.querySelector('.et-hero-tab-slider');
        if (slider && this.currentTab) {
            slider.style.width = `${this.currentTab.offsetWidth}px`;
            slider.style.left = `${this.currentTab.offsetLeft}px`;
        }
    }

    initBootstrapDropdowns() {
        if (window.bootstrap) { 
            document.querySelectorAll('.dropdown-toggle').forEach((element) => {
                new bootstrap.Dropdown(element);
            });
        }
    }

    debounce(func, wait) {
        let timeout;
        return function (...args) {
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(this, args), wait);
        };
    }
}

// Ensure StickyNavigation does not interfere with Bootstrap dropdown
document.addEventListener('DOMContentLoaded', () => new StickyNavigation());
