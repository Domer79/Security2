function States() {
    this.adminPanelState = {
        name: 'adminpanel',
        url: '/adminpanel',
        abstract: true,
        views: {
            "main": {
                component: "adminpanel"
            }
        },
        data: {
            tabs2: {
                0: "tab0",
                1: "tab1",
                2: "tab2"
            }
        }
    };
}