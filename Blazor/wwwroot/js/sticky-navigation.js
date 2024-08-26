class StickyNavigation {
    constructor() {
        this.currentId = null;
        this.currentTab = null;
        this.tabContainerHeight = 70;
        let self = this;
        document.querySelectorAll('.et-hero-tab').forEach(tab => {
            tab.addEventListener('click', (event) => self.onTabClick(event, tab));
        });
        window.addEventListener('scroll', () => this.onScroll());
        window.addEventListener('resize', () => this.onResize());
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
        let self = this;
        document.querySelectorAll('.et-hero-tab').forEach(tab => {
            let id = tab.getAttribute('href');
            let offsetTop = document.querySelector(id).offsetTop - self.tabContainerHeight;
            let offsetBottom = document.querySelector(id).offsetTop + document.querySelector(id).offsetHeight - self.tabContainerHeight;
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
        document.querySelector('.et-hero-tab-slider').style.width = width;
        document.querySelector('.et-hero-tab-slider').style.left = left;
    }
}

document.addEventListener('DOMContentLoaded', () => new StickyNavigation());
