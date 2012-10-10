window.Tests = window.Tests || {};

(function (Tests) {
    var jasmineEnv,
        htmlReporter;
        
    htmlReporter = new jasmine.HtmlReporter();

    jasmineEnv = jasmine.getEnv();
    jasmineEnv.updateInterval = 250;
    jasmineEnv.addReporter(htmlReporter);
    jasmineEnv.specFilter = function (spec) {
        return htmlReporter.specFilter(spec);
    };

    Tests.run = function () {
        jasmineEnv.execute();
    };

    describe("Bootstrapper tests", function () {
        beforeEach(function () {
            //Clear bootstrap stuff so tests make more sense
            Bootstrap._loaded = [];
            Bootstrap._loading = [];
        });

        describe("A call to Bootstrap.load for Backbone", function () {
            var loaded = false;

            beforeEach(function () {
                spyOn(Bootstrap, "_getJsPath").andCallThrough();
                Bootstrap.load("backbone", function () {
                    loaded = true;
                });
                
                waitsFor(function () {
                    return loaded;
                }, "Backbone to load", 1000);
            });
            
            it("should load jquery", function () {
                expect(jQuery).not.toBeUndefined();
            });
            
            it("should load underscore", function () {
                expect(_).not.toBeUndefined();
            });
            
            it("should load backbone", function () {
                expect(Backbone).not.toBeUndefined();
            });
            
            it("should call Bootstrap._getJsPath", function () {
                expect(Bootstrap._getJsPath).toHaveBeenCalled();
            });

            it("should call Bootstrap_.getJsPath 3 times", function () {
                expect(Bootstrap._getJsPath.calls.length).toEqual(3);
            });
        });
    });
})(window.Tests);

Bootstrap.wait(Tests.run);