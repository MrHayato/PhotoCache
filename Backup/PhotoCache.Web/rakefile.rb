
desc "Compiles SCSS/SASS assets using compass"
desc "Updates and installs gems"
task :bundle_install do
  sh "bundle install"
end

task :compass => :bundle_install do
  require 'compass'
  require 'compass/exec'
  Compass::Exec::SubCommandUI.new(['compile', '--force', '-s', 'compressed']).run!
end

namespace :compass do
  desc "Watches for changes SASS assets using compass watch"
  task :watch do
    require 'compass'
    require 'compass/exec'
    Compass::Exec::SubCommandUI.new(['watch']).run!
  end
end

task :default => [ :bundle_install, :compass]

