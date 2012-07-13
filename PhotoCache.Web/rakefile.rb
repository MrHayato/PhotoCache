#TEST_RESULTS = "#{OUTPUT}/tests"

#desc "Tests, does not compile. Only has machine.specifications."
#task :test => :mspec
#
#desc "Create test result directory and tell TeamCity about it"
#task :testOutput => TEST_RESULTS do
#  artifact TEST_RESULTS
#end
#
#desc "Runs machine.specifications. Does not compile."
#mspec :mspec => :testOutput do |mspec|
#  specs = "#{TEST_RESULTS}/#{PRODUCT_NAME}-specs.html"
#	mspec.command = tool "Machine.Specifications", "mspec-clr4.exe"
#	mspec.assemblies FileList.new("#{BASE_DIR}/*/bin/*/*Specs.dll")
#	mspec.html_output = "#{specs}"
#  mspec.options '--teamcity' if IS_TEAMCITY
#end
#
#desc "Compiles SCSS/SASS assets using compass"
#task :compass do
#  require 'compass'
#  require 'compass/exec'
#  Compass::Exec::SubCommandUI.new(['compile', '--force', 'IQ.IR.Web/media/']).run!
#end
#
#namespace :db do
#  desc 'Reset the all passwords to "Password"'
#  task :password_reset do
#    sh 'MongoWorker\MongoDBBinaries\mongo.exe xq ./tools/scripts/reset_passwords.js'
#  end
#
#  desc 'Drop the database'
#  task :drop => :output do
#    sh 'MongoWorker\MongoDBBinaries\mongo.exe xq ./tools/scripts/drop_database.js'
#  end
#end