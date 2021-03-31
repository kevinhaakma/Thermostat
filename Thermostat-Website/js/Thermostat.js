const sliderMinX = 0
const sliderMaxX = 240

const coldGradient = { start: '#5564C2', end: '#3A2E8D' }
const hotGradient = { start: '#F0AE4B', end: '#9B4D1B' }

new Vue({
    el: '#app',
    data: {
        dragging: false,
        initialMouseX: 0,
        sliderX: 0,
        initialSliderX: 0,
        temperatureGrades: [15, 17, 20, 22, 25],
        gradientStart: coldGradient.start,
        gradientEnd: coldGradient.end,
        APIdesiredTemperature: -1.1,
        APIcurrentTemperature: -1.1,
        APIheating: false,
    },
    filters: {
        round(num) {
            return (Math.round(num * 10) / 10).toFixed(1);
        }
    },
    methods: {
        startDrag(e) {
            this.dragging = true
            this.initialMouseX = e.pageX
            this.initialSliderX = this.sliderX
        },
        stopDrag() {
            this.dragging = false;
            this.setTemperature();
        },
        mouseMoving(e) {
            if (this.dragging) {
                const dragAmount = e.pageX - this.initialMouseX
                const targetX = this.initialSliderX + dragAmount

                // keep slider inside limits
                this.sliderX = Math.max(Math.min(targetX, sliderMaxX), sliderMinX)
            }
        },
        async UpdateData() {
            fetch('http://192.168.1.1:5000/Thermostat/Status', { headers: { 'Access-Control-Allow-Origin': '*' } })
                .then(response => response.json())
                .then(data => {
                    this.APIcurrentTemperature = data.currentTemperature;
                    this.APIheating = data.heating;
                    if (!this.dragging) {
                        this.APIdesiredTemperature = (Math.round(data.desiredTemperature * 10) / 10).toFixed(1);
                        this.sliderX = (this.APIdesiredTemperature - 15) * 24;
                    }
                })

            // set bg color
            let targetGradient = coldGradient
            if (this.APIheating) {
                targetGradient = hotGradient
            }

            if (this.gradientStart !== targetGradient.start) {
                // gradient changed
                TweenLite.to(this, 0.7, {
                    'gradientStart': targetGradient.start,
                    'gradientEnd': targetGradient.end
                })
            }
        },
        async setTemperature() {
            fetch('http://192.168.1.1:5000/Thermostat/Temperature?temperature=' + (Math.round(this.desiredTemperature * 10) / 10).toFixed(1).toString(), {
                headers: { 'Access-Control-Allow-Origin': '*' },
                method: 'PUT'
            })
        },
        tempElementStyle(tempNumber) {
            const nearDistance = 3
            const liftDistance = 12

            // lifts up the element when the current temperature is near it
            const diff = Math.abs(this.desiredTemperature - tempNumber)
            const distY = (diff / nearDistance) - 1

            // constrain the distance so that the element doesn't go to the bottom
            const elementY = Math.min(distY * liftDistance, 0)
            return `transform: translate3d(0, ${elementY}px, 0)`
        }
    },
    computed: {
        desiredTemperature() {
            const tempRangeStart = 15
            const tempRange = 10 // from 10 - 30
            return (this.sliderX / sliderMaxX * tempRange) + tempRangeStart
        },
        currentTemperature() {
            return this.APIcurrentTemperature;
        },
        sliderStyle() {
            return `transform: translate3d(${this.sliderX}px,0,0)`
        },
        bgStyle() {
            return `background: linear-gradient(to bottom right, ${this.gradientStart}, ${this.gradientEnd});`
        }
    },
    created() {
        this.UpdateData();
        setInterval(this.UpdateData, 1000);
    }
})