const colors = require('tailwindcss/colors');

export default {
    content: ["./src/**/*.{html,js,ts,tsx}"],
    theme: {
        colors: {
            'transparent': 'transparent',
            'gray': colors.gray,
            'rose': colors.rose,
            'sky': colors.sky,
            'yellow': colors.yellow,
            'red': colors.red,
            'amber': colors.amber,
            'bg1': '#f7f7f7',
            'bg2': '#000001',
            'bg3': '#e0e0e0',
            'bg4': '#d2d2d2',
            'bg5': "#aeaeae",
            'text1': '#373332',
            'text2': '#fafafa',
            'text3': '#696463',
            'accent1': '#dbc123',
            'accent2': '#8b2c3e'
        },
        extend: {}
    },
    plugins: [],
    extend: {
    }
};