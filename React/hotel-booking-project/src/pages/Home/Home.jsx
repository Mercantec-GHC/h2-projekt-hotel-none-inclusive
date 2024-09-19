import './Home.css'
import testimonials from "./Testimonials/testimonials.js"
import TestimonialItem from "../../components/Testimonials/TestimonialItem.jsx";
import facilities from "./Facilities/facilities.jsx"
import FacilityItem from "../../components/Facility/FacilityItem.jsx";
import Marquee from "react-marquee-slider";

function Home() {
    return (
        <>
            <div className="homepage-background-image-container" draggable="false">
                <img src="src/assets/Hotel.jpg" alt="Hotel image" draggable="false"
                     className="homepage-background-image"/>
            </div>

            <section className="homepage-first-section" draggable="false">
                <h1 className="homepage-first-section-title">Velkommen til None Inclusive</h1>
                <p className="homepage-first-section-subtitle">Oplev uovertruffen komfort og elegance i hjertet af
                    byen.</p>
                <button className="homepage-first-section-button">Book din oplevelse</button>
            </section>

            <section className="homepage-second-section" draggable="false">
                <div className="homepage-second-section-facility-container">
                    <h1 className="homepage-second-section-facility-title">Faciliteter</h1>
                    <Marquee velocity={25}>
                        {facilities.map((facility) => {
                                return (
                                    <FacilityItem
                                        key={facility.id}
                                        icon={facility.icon}
                                        facility={facility.facility}
                                        nameForClass="homepage-second-section-facility-item"
                                        nameForText="homepage-second-section-facility-text"
                                    />
                                )
                            }
                        )}
                    </Marquee>

                </div>
                <div className="homepage-second-section-testimonials-container">
                    <h1 className="homepage-second-section-testimonials-title">Hvad siger vores gæster?</h1>
                    <Marquee direction={"ltr"} velocity={5}>
                        {testimonials.map((testimonial) => {
                                return (
                                    <TestimonialItem
                                        key={testimonial.id}
                                        name={testimonial.name}
                                        country={testimonial.country}
                                        text={testimonial.text}
                                        nameForClass="homepage-second-section-testimonial-item"
                                        nameForText="homepage-second-section-testimonial-text"
                                    />
                                )
                            }
                        )}
                    </Marquee>
                </div>
            </section>
        </>
    )
}

export default Home