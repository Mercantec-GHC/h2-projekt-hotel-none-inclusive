import './Home.css'


function Home() {
    return (
        <>
            <div className="homepage-background-image-container" draggable="false">
                <img src="src/assets/shubham-dhage-T9rKvI3N0NM-unsplash.jpg" alt="AdminImage" draggable="false"
                     className="homepage-background-image"/>
            </div>

            <section className="homepage-first-section" draggable="false">
                <h1 className="homepage-first-section-title">Admin</h1>
                <p className="homepage-first-section-subtitle">None Inclusive Admin-portal</p>

            </section>

           
        </>
    )
}

export default Home